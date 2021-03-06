﻿// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Data.Entity.Metadata;
using Fireasy.Data.Syntax;
using System.Linq;
using System.Text;

namespace Fireasy.Data.Entity.Generation
{
    public class SQLiteTableGenerator : BaseTableGenerateProvider
    {
        protected override SqlCommand[] BuildCreateTableCommands(ISyntaxProvider syntax, EntityMetadata metadata, IProperty[] properties)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("create table main.{0}\n(\n", Quote(syntax, metadata.TableName));

            var count = properties.Length;
            for (var i = 0; i < count; i++)
            {
                AppendFieldToBuilder(sb, syntax, properties[i]);

                if (i != count - 1)
                {
                    sb.Append(",");
                }

                sb.AppendLine();
            }

            sb.Append(");\n");

            return new SqlCommand[] { sb.ToString() };
        }

        protected override SqlCommand[] BuildAddFieldCommands(ISyntaxProvider syntax, EntityMetadata metadata, IProperty[] properties)
        {
            var sb = new StringBuilder();
            var count = properties.Length;
            for (var i = 0; i < count; i++)
            {
                sb.AppendFormat("alter table {0} add ", Quote(syntax, metadata.TableName));

                AppendFieldToBuilder(sb, syntax, properties[i]);

                sb.AppendLine(";");
            }

            return new SqlCommand[] { sb.ToString() };
        }

        protected override void ProcessPrimaryKeyField(StringBuilder builder, ISyntaxProvider syntax, IProperty property)
        {
            builder.Append(" primary key");
        }
    }
}
