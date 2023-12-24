# Danfe Gen API

Este é um projeto de exemplo escrito em C# que oferece uma API para a geração de DANFE (Documento Auxiliar da Nota Fiscal Eletrônica) utilizando a biblioteca [DanfeSharp](https://github.com/SilverCard/DanfeSharp).


## Instalação

Clone o Repositório:

```bash
git clone https://github.com/giovani-plantae/DanfeGenAPI.git
```

Acesse o Diretório do Projeto:
```bash
cd DanfeGenAPI
```

Restaure as Dependências:
```bash
dotnet restore
```

Execute o Projeto:
```bash
dotnet run
```
Isso irá baixar as dependências necessárias e iniciar a aplicação.


## Utilização

Requisição:
- Método: POST
- URL: http://localhost:5000/danfe
- Tipo de Conteúdo: multipart/form-data
- Parâmetros:
    - invoice: Arquivo XML contendo os dados da nota fiscal.
    - logo (opcional): Arquivo JPG ou PNG com a logo do cliente.

Exemplo de Requisição usando cURL:
```bash
curl -X POST \
-H "Content-Type: multipart/form-data" \
-F "invoice=@caminho/do/arquivo.xml" \
-F "logo=@caminho/da/logo.png" \
http://localhost:5000/danfe -o danfe.pdf
```
Resposta Esperada:
Após a requisição bem-sucedida, você fará o download do arquivo PDF contendo a DANFE.


### Observações:

O parâmetro logo é opcional. Se não for fornecido, a DANFE será gerada sem a inclusão da logo do cliente. Caso seja fornecido, a logo será incorporada ao PDF gerado.

Exemplo de Requisição sem Logo:
```bash
curl -X POST \
-H "Content-Type: multipart/form-data" \
-F "invoice=@caminho/do/arquivo.xml" \
http://localhost:5000/danfe -o danfe.pdf
```
