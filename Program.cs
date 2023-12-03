using System;
using System.Data;
using System.Formats.Asn1;
using System.Reflection;
using Dapper;
using DidimboteDataAccess.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

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
                // ExecuteProcedure(connection);
                // ListCategory(connection);
                // ExecuteReadProcedure(connection);
                // GetCategory(connection);
                // ExecuteEscalar(connection);
                // ReadView(connection);
                // OneToOne(connection);
                // OneToMany(connection);
                // QueryMultiple(connection);
                // SelectIn(connection);
                // Like(connection);
                //Like_opction2(connection, "asp");
                Transicion(connection);
            }
        }
        static void ListCategory(SqlConnection connection)
        {
            //Criando uma lista a partir da query e salvando na variavel CATEGORIES
            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]"); //Lista do tipo Tipada como CATEGORY

            //Percorrendo os itens da lista
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void GetCategory(SqlConnection connection)
        {
            var category = connection
                .QueryFirstOrDefault<Category>(
                    "SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id]=@id",
                    new
                    {
                        id = "af3407aa-11ae-4621-a2ef-2028b85507c4"
                    });
            Console.WriteLine($"{category?.Id} - {category?.Title}");

        }
        static void CreateCategory(SqlConnection connection)
        {

            // Atribuindo valores as propriedades para depois inserir na Base de Dados
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Summary = "AWS Cloud";
            category.Order = 8;
            category.Description = "Categoria destinada a serviços do AWS";
            category.Featured = false;

            // Query para criar categoria
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

            // Executa o insert com os parametros e guarda o numero de linhas inserida na variavel ROWS
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

            var rows = connection.Execute(UpdateQuery, new[] // E' um array de parametro, ou seja, 
                                                             // executa a query varias vezes, mudando os parametros
            {
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
        static void ExecuteProcedure(SqlConnection connection)
        {
            // Informa a StoreProcedure a ser executada
            var procedure = "[spDeleteStudent]";

            // Informa o parametro da Procedure
            var parametroProcedure = new { StudentId = "5831a86b-c7e7-49d4-b28b-8af961efea1e" };

            // Executa a Procedure e armazena o numero de linhas afetadas na variavel 
            var affetedRows = connection.Execute(procedure,
                parametroProcedure,
                commandType: CommandType.StoredProcedure);

            // Imprime as linhas afectadas
            Console.WriteLine($"{affetedRows} linhas afetadas");
        }

        static void ExecuteReadProcedure(SqlConnection connection)
        {
            // Informa a StoreProcedure a ser executada
            var procedure = "[spGetCoursesByCategory]";

            // Informa o parametro da Procedure
            var parametroProcedure = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };

            // Executa a Procedure de leitura e armazena os dados na variavel 
            var courses = connection.Query(procedure, //Lista do tipo DINAMICA
                parametroProcedure,
                commandType: CommandType.StoredProcedure);

            // Percorre a lista dinamica de cursos Imprime os cursos na tela
            foreach (var item in courses)
            {
                Console.WriteLine($"{item.Id}");
            }
        }

        static void ExecuteEscalar(SqlConnection connection)
        {
            // Atribuindo valores as propriedades para depois inserir na Base de Dados
            var category = new Category();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Summary = "AWS Cloud";
            category.Order = 8;
            category.Description = "Categoria destinada a serviços do AWS";
            category.Featured = false;

            // Query para criar categoria
            var insertSql = @"INSERT INTO 
                        [Category] 
                        OUTPUT inserted.[Id]
                    VALUES (
                        NEWID(),
                        @Title,
                        @Url,
                        @Summary,
                        @Order,
                        @Description,
                        @Featured) ";

            // Executa o insert com os parametros e guarda o numero de linhas inserida na variavel ROWS
            var id = connection.ExecuteScalar<Guid>(insertSql, new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"A categoria inserida tem o id: {id}");
        }

        static void ReadView(SqlConnection connection)
        {
            var sql = "SELECT * FROM [VwCourses]";
            var Courses = connection.Query(sql); //Lista do tipo dinamica

            //Percorrendo os itens da lista
            foreach (var item in Courses)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }
        static void OneToOne(SqlConnection connection)
        {
            var sql = @"
                SELECT 
                    * 
                FROM 
                    [CareerItem]
                INNER JOIN
                    [Course]
                ON 
                    [CareerItem].[CourseId] = [Course].[Id]";


            var items = connection.Query<CareerItem, Course, CareerItem>(
                sql,
                (careerItem, course) =>
                {
                    careerItem.Course = course;
                    return careerItem;
                }, splitOn: "Id");

            foreach (var item in items)
            {
                Console.WriteLine(item.Title);
            }
        }
        static void OneToMany(SqlConnection connection)
        {
            var sql = @"
                SELECT 
                    [Career].[Id],
                    [Career].[Title],
                    [CareerItem].[CareerId],
                    [CareerItem].[Title]
                FROM
                    [Career]
                INNER JOIN
                    [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
                ORDER BY
                    [Career].[Title]";

            // Criando uma lista de carreira
            var careers = new List<Career>();

            // Carrega os dados do inner join e atribui nos parametros careere item, e posteriormente atribuiu a variavel careers
            var items = connection.Query<Career, CareerItem, Career>(
                sql,
                (career, item) =>
                {
                    // Verifica se o item ja existe na carreira
                    var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();

                    // Se nao encontrar nada esse item vai ser nulo
                    if (car == null)
                    {
                        //Adiciona item a lista carreira
                        car = career;// Atribui o valor da carreira a variavel car
                        car.Items.Add(item); // Adiciona um "item" a variavel car que ja tem uma carreira
                        careers.Add(car);// A carreira e o seu item sao adicionado a lista de carreiras
                    }
                    // Caso a carreira ja esteja na lista
                    else
                    {
                        car.Items.Add(item); // Adiciona um "item" a variavel car que ja tem uma carreira
                    }

                    return career;
                }, splitOn: "CareerId");

            // Percorre as carreiras
            foreach (var career in careers)
            {
                Console.WriteLine($"{career.Title}");

                // Percorre os itens da carreira
                foreach (var item in career.Items)
                {
                    Console.WriteLine($"{item.Title}");
                }
            }
        }
        static void QueryMultiple(SqlConnection connection)
        {
            // Funcao para fazer multiplos SELECTs

            // Declarando a variavel para receber mais de um SELECT
            var sqlquery = "SELECT * FROM [Category]; SELECT * FROM [Course]";

            //Executando a query multipla ou multiplos SELECTs
            using (var multi = connection.QueryMultiple(sqlquery))
            {
                // Pode ser feito um OneToMany e/ou OneToOne dentro do QueryMultiple

                // Armazenar os valores nas novas variaveis
                var categories = multi.Read<Category>(); // Lista do tipo tipada
                var courses = multi.Read<Course>(); // Lista do tipo tipada

                // Percorre todos titulos da categoria
                foreach (var item in categories)
                {
                    Console.WriteLine(item.Title);
                }

                // Percorre todos titulos do curso
                foreach (var item in courses)
                {
                    Console.WriteLine($"{item.Title}");
                }
            }
        }
        static void SelectIn(SqlConnection connection)
        {
            // Declarando a viriavel que carrega o comando sql
            var sqlquery = "SELECT * FROM [Career] WHERE [Id] IN @Id";

            // Executa a query e armazena os dados na variavel item
            var items = connection.Query<Career>(sqlquery, new // Executa a mesma query, mudando os parametros,
                                                               // Ou seja, e' um parametro com um array dentro
            {
                // Cria um array de parametros
                Id = new[]{
                    "4327ac7e-963b-4893-9f31-9a3b28a4e72b",
                    "e6730d1c-6870-4df3-ae68-438624e04c72"
                }
            });

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title}");
            }
        }
        static void Like(SqlConnection connection)
        {
            // Declarando o termo de busca
            var term = "node";

            // Declarando a viriavel que carrega o comando sql
            var sqlquery = "SELECT * FROM [Course] WHERE [Title] LIKE @exp";

            // Executa a query e armazena os dados na variavel item
            var items = connection.Query<Career>(sqlquery, new // Executa a mesma query, mudando os parametros,
                                                               // Ou seja, e' um parametro com um array dentro
            {
                // Declarando o TERM como parametro SQL de busca
                exp = $"%{term}%"
            });

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title}");
            }
        }

        static void Like_opction2(SqlConnection connection, string term)// Like com parametro SQL termo
        {
            // Declarando a viriavel que carrega o comando sql
            var sqlquery = "SELECT * FROM [Course] WHERE [Title] LIKE @exp";

            // Executa a query e armazena os dados na variavel item
            var items = connection.Query<Career>(sqlquery, new // Executa a mesma query, mudando os parametros,
                                                               // Ou seja, e' um parametro com um array dentro
            {
                // Declarando o TERM como parametro SQL de busca
                exp = $"%{term}%"
            });

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title}");
            }
        }

        static void Transicion(SqlConnection connection)
        {
            // Atribuindo valores as propriedades para depois inserir na Base de Dados
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Minha nova conta";
            category.Url = "amazon";
            category.Summary = "AWS Cloud";
            category.Order = 8;
            category.Description = "Categoria destinada a serviços do AWS";
            category.Featured = false;

            // Query para criar categoria
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

            // Abrir a conexao
            connection.Open();

            // Inicia a TRANSITION
            using (var transaction = connection.BeginTransaction())
            {
                // Executa o insert com os parametros e guarda o numero de linhas inserida na variavel ROWS
                var rows = connection.Execute(insertSql, new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                }, transaction);

                //transaction.Commit(); // Executa o insert

                transaction.Rollback(); // Desfaz tudo o que ele fez

                Console.WriteLine($"{rows} linhas inseridas");
            }
        }
    }
}