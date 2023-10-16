using System;
using System.Formats.Asn1;
using System.Reflection;
using Dapper;
using DidimboteDataAccess.Models;
using Microsoft.Data.SqlClient;

namespace DidimboteDataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True";

            using (var connection = new SqlConnection(connectionString))
            {
                // DeleteQuery(connection);
                // UpdateQuery(connection);
                // CreateCategory(connection);
                // CreateManyCategory(connection);
                // DeleteManyQuery(connection);
                // UpdateManyQuery(connection);
                // ListCategory(connection);
            }
        }
        static void ListCategory(SqlConnection connection)
        {
            //Criando uma lista a partir da query e salvando na variavel CATEGORIES
            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
            //Percorrendo os itens da lista

            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }
        static void CreateCategory(SqlConnection connection)
        {

            //Atribuindo valores as propriedades para depois inserir na Base de Dados
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Summary = "AWS Cloud";
            category.Order = 8;
            category.Description = "Categoria destinada a serviços do AWS";
            category.Featured = false;

            //PQuery para criar categoria
            var insertSql = @"INSERT INTO 
                        [Category] 
                    VALUES (
                        @Id,
                        @Title,
                        @Url,
                        @Summary,
                        @Order,
                        @Description,
                        @Featured)";

            //Executa o insert com os parametros e guarda o numero de linhas inserida na variavel ROWS
            var rows = connection.Execute(insertSql, new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"{rows} linhas inseridas");
        }
        static void UpdateQuery(SqlConnection connection)
        {
            var UpdateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@id";
            var rows = connection.Execute(UpdateQuery, new
            {
                id = new Guid("8a28148a-c66a-4265-857b-25c1215f0604"),
                title = "Didimbote"
            });

            Console.WriteLine($"{rows} registros actualizados");
        }
        static void DeleteQuery(SqlConnection connection)
        {
            var DeleteQuery = "DELETE FROM [Category] WHERE [Id]=@id";
            var rows = connection.Execute(DeleteQuery, new
            {
                id = new Guid("00000000-0000-0000-0000-000000000000")
            });
            Console.WriteLine($"{rows} registros deletados.");
        }
        static void CreateManyCategory(SqlConnection connection)
        {
            var category = new Category();

            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Summary = "AWS Cloud";
            category.Order = 8;
            category.Description = "Categoria destinada a serviços do AWS";
            category.Featured = false;

            var category2 = new Category();

            category2.Id = Guid.NewGuid();
            category2.Title = "Categoria nova";
            category2.Url = "categoria-nova";
            category2.Summary = "categoria nova Cloud";
            category2.Order = 9;
            category2.Description = "Categoria nova";
            category2.Featured = true;

            var insertSql = @"INSERT INTO [Category]
                VALUES(
                    @Id,
                    @Title,
                    @Url,
                    @Summary,
                    @Order,
                    @Description,
                    @Featured)";

            var rows = connection.Execute(insertSql, new[]
            {
                new{
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            },
                new{
                    category2.Id,
                    category2.Title,
                    category2.Url,
                    category2.Summary,
                    category2.Order,
                    category2.Description,
                    category2.Featured
                }
            });
            Console.WriteLine($"{rows} linhas de registros inseridos");
        }
        static void DeleteManyQuery(SqlConnection connection)
        {
            var deleteSql = "DELETE FROM [Category] WHERE [Id]=@id";

            var rows = connection.Execute(deleteSql, new[]{
                new{
                    id = new Guid("5c7c005c-204e-4f19-837a-52fa65446760")
                },
                new{
                    id = new Guid("4b62a659-f494-45e2-8082-733215806267")
                }
            });
            Console.WriteLine($"{rows} linhas de registros deletados.");
        }
        static void UpdateManyQuery(SqlConnection connection)
        {
            var UpdateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@id";

            var rows = connection.Execute(UpdateQuery, new[]{
                new{
                    id = new Guid("5c7c005c-204e-4f19-837a-52fa65446760"),
                    title = "Peter Fernandes"
                }, new{
                    id= new Guid("4b62a659-f494-45e2-8082-733215806267"),
                    title = "Afonsina Fernandes"
                }
            });
            Console.WriteLine($"{rows} linhas de registros actualizadas");
        }
    }
}