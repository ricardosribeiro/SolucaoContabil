namespace SolucaoContabil.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cod = c.Int(nullable: false),
                        Descricao = c.String(maxLength: 150, unicode: false),
                        ClienteTipoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClienteTipo", t => t.ClienteTipoId)
                .Index(t => t.ClienteTipoId);
            
            CreateTable(
                "dbo.ClienteTipo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descricao = c.String(maxLength: 150, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lancamento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Competencia = c.String(maxLength: 150, unicode: false),
                        ValorDebito = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorCredito = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClienteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cliente", t => t.ClienteId)
                .Index(t => t.ClienteId);
            
            CreateTable(
                "dbo.SpedA",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        CodigoTaxa = c.String(maxLength: 150, unicode: false),
                        DescricaoTaxa = c.String(maxLength: 150, unicode: false),
                        Competencia = c.String(maxLength: 150, unicode: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpedZ",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        CodigoImovelProprietario = c.String(maxLength: 150, unicode: false),
                        TipoLancamento = c.String(maxLength: 150, unicode: false),
                        CodigoTaxa = c.String(maxLength: 150, unicode: false),
                        DescricaoTaxa = c.String(maxLength: 150, unicode: false),
                        ComplementoTaxa = c.String(maxLength: 150, unicode: false),
                        Competencia = c.String(maxLength: 150, unicode: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lancamento", "ClienteId", "dbo.Cliente");
            DropForeignKey("dbo.Cliente", "ClienteTipoId", "dbo.ClienteTipo");
            DropIndex("dbo.Lancamento", new[] { "ClienteId" });
            DropIndex("dbo.Cliente", new[] { "ClienteTipoId" });
            DropTable("dbo.SpedZ");
            DropTable("dbo.SpedA");
            DropTable("dbo.Lancamento");
            DropTable("dbo.ClienteTipo");
            DropTable("dbo.Cliente");
        }
    }
}
