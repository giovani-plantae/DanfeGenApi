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

A aplicação disponibiliza a rota `/danfe` do tipo `POST` que aceita dados no formato `multipart/form-data`, permitindo o envio de um arquivo XML para geração do PDF da DANFE.

Exemplo de requisição usando `cURL`:
```bash
curl -X POST -H "Content-Type: multipart/form-data" -F "invoice=@caminho/do/arquivo.xml" http://localhost:5000/danfe -o danfe.pdf
```

Resposta Esperada:

Após a requisição, se tudo deu certo, você terá feito download do arquivo PDF da DANFE.
