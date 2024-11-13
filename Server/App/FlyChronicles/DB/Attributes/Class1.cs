using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Attributes
{
    internal class Class1
    {
    }

    /// <summary>
    /// Атрибут Имя таблицы (используется в контексте БД в методе создания моделей)
    /// </summary>
    public class TableNameAttribute : Attribute
    {
        /// <summary>
        /// Наименование таблицы
        /// </summary>
        public string Name { get; }

        public TableNameAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Атрибут - первичный ключ
    /// </summary>
    public class PrimaryKeyAttribute : Attribute
    {

    }

    /// <summary>
    /// Игнорирование поля
    /// </summary>
    public class IgnoreAttribute : Attribute
    {
    }

    /// <summary>
    /// Атрибут - тип колонки
    /// </summary>
    public class ColumnTypeAttribute : Attribute
    {
        /// <summary>
        /// Наименование типа колонки
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        public ColumnTypeAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Атрибут Имя колонки БД
    /// </summary>
    public class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// Имя колоник БД
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        public ColumnNameAttribute(string name)
        {
            Name = name;
        }
    }
}
