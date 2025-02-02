﻿using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace FlyCronicles.Common.Options
{
    public class ErrorNotifyService : IDisposable, IErrorNotifyService
    {
        private bool isConnected = false;
        private bool isAuth = false;
        private bool isDisposed = false;
        private bool _sendMessage = false;

        private string _server;
        private string _login;
        private string _password;
        private string _feedback;
        private string _defaultTitle;

        private object _lockObject = new object();
        private bool isLock = false;

        private string _token { get; set; }

        public ErrorNotifyService(ErrorNotifyOptions options)
        {
            if (options.SendError)
            {
                if (!string.IsNullOrEmpty(options.Server))
                {
                    _sendMessage = true;
                    _server = options.Server;
                    _login = options.Login;
                    _password = options.Password;
                    _feedback = options.FeedbackContact;
                    _defaultTitle = options.DefaultTitle;

                    Task.Factory.StartNew(CheckConnect, TaskCreationOptions.LongRunning);
                }
                else
                {
                    Console.WriteLine($"ErrorNotifyService error: Options.Server not set");
                }
            }
        }

        private async Task<bool> Auth()
        {
            bool _isLocked = false;
            lock (_lockObject)
            {
                if (isLock)
                {
                    _isLocked = true;
                }
                else
                {
                    isLock = true;
                }
            }
            if (_isLocked)
            {
                for (int i = 0; i < 60; i++)
                {
                    if (!isLock)
                    {
                        break;
                    }
                    await Task.Delay(1000);
                }
                if (!isLock)
                {
                    if (isAuth) return true;
                    if (isConnected) return false;
                }
                else
                {
                    Console.WriteLine($"ErrorNotifyService: Error in Auth method: cant wait for auth with lock");
                    return false;
                }
            }

            var result = await Execute(client =>
                client.PostAsync($"{_server}/api/v1/client/auth", new ErrorNotifyClientIdentity()
                {
                    Login = _login,
                    Password = _password
                }.SerializeRequest()), "Post", s => s.ParseResponse<ErrorNotifyClientIdentityResponse>(), false);
            if (result.ResponseCode == ResponseEnum.Error)
            {
                if (isConnected)
                {
                    Console.WriteLine($"ErrorNotifyService: Error in Auth method: wrong login or password");
                    _sendMessage = false;
                }
                return false;
            }
            _token = result.ResponseBody.Token;
            isAuth = true;
            lock (_lockObject)
            {
                isLock = false;
            }
            return true;
        }

        public async Task Send(string message, MessageLevelEnum level = MessageLevelEnum.Error, string title = null)
        {
            if (_sendMessage)
            {
                var result = await Execute(client =>
                {
                    var request = new HttpRequestMessage()
                    {
                        Headers = {
                            { HttpRequestHeader.Authorization.ToString(), $"Bearer {_token}" },
                            { HttpRequestHeader.ContentType.ToString(), "application/json" },
                        },
                        RequestUri = new Uri($"{_server}/api/v1/message/send"),
                        Method = HttpMethod.Post,
                        Content = new MessageCreator()
                        {
                            Description = message,
                            FeedbackContact = _feedback,
                            Level = (int)level,
                            Title = title ?? _defaultTitle
                        }.SerializeRequest()
                    };

                    return client.SendAsync(request);
                }, "Send", s => s.ParseResponse<MessageCreator>());

                if (result.ResponseCode == ResponseEnum.Error)
                {
                    Console.WriteLine($"ErrorNotifyService: Error in Send method: cant send message error");
                }
            }
        }

        private async Task<Response<T>> Execute<T>(
            Func<HttpClient, Task<HttpResponseMessage>> action,
            string method,
            Func<HttpResponseMessage, Task<Response<T>>> parseMethod, bool needAuth = true) where T : class
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (isConnected)
                    {
                        var result = await action(client);
                        var resp = await parseMethod(result);
                        if (resp.ResponseCode == ResponseEnum.NeedAuth)
                        {
                            if (needAuth && await Auth())
                            {
                                result = await action(client);
                                resp = await parseMethod(result);
                            }
                            else
                            {
                                return new Response<T>()
                                {
                                    ResponseCode = ResponseEnum.Error
                                };
                            }
                        }
                        return resp;
                    }
                    Console.WriteLine($"Error in {method}: server not connected");
                    return new Response<T>()
                    {
                        ResponseCode = ResponseEnum.Error
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in {method}: {ex.Message}; StackTrace: {ex.StackTrace}");
                    return new Response<T>()
                    {
                        ResponseCode = ResponseEnum.Error
                    };
                }
            }
        }

        private async Task CheckConnect()
        {
            while (!isDisposed)
            {
                isConnected = await CheckConnectOnce(_server);
                await Task.Delay(1000);
            }
        }

        private async Task<bool> CheckConnectOnce(string server)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var check = await client.GetAsync($"{server}/api/v1/common/ping");
                    var result = check != null && check.IsSuccessStatusCode;
                    Console.WriteLine($"Ping result: server {server} {(result ? "connected" : "disconnected")}");
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in CheckConnect: {ex.Message}; StackTrace: {ex.StackTrace}");
                    return false;
                }
            }
        }

        public void Dispose()
        {
            isDisposed = true;
        }
    }
}
