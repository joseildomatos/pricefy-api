
## Instruções gerais
### Arquitetura
- .Net Framework = 4.6
- .Net Core      = 3.1
- Sql Server     = 2012
- IDE            = Visual Studio 2019*
### Banco de dados
- Autenticação   = Windows com perfil adm(padrao)
- Nome           = Pricefy (criado antes do inicialiazação dos testes - pode ser qualquer outro, bastando editar a ConnectionString)
- Tabelas        =  << Serão solicitadas e criadas automaticamente no ato da carga >>

## Carga
### Arquivo .CSV
- Nome          = ```ExemploPriceFy.scv```
                Obs.: - O mesmo deve ficar dentro de uma pasta chamada 'data' na 'raiz' do executável
                      - Pode-se realizar quantos testes forem necessarios, a rotina sempre irá recriar as tabelas
                ```Ex.:~/data/ExemploPriceFy.csv```
### Realizando carga
- Abrir fonte e editar a ```ConnectionString``` para o desejado, no arquivo : Program.cs
- Abrir fonte e editar a ```arquivoCSVtPath``` para o desejado, no arquivo : Program.cs
- Executra teste (F5) - obs: serão solicitadas algumas informações como delimitador e tabelas
## Api - recuperando JSON(paginação)
- No navegador, inserir a Url = ```https://localhost:{porta}/api/importacao/paginacao?numeroPagina=1&LimitePagina=20```
                               Obs.: se for executar em mode de depuração(F5), o sistema irá sugerir a porta
## Repositórios
- https://github.com/joseildomatos/pricefy-carga-csv.git
- https://github.com/joseildomatos/pricefy-api.git
