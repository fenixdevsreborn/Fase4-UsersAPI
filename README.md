# Users API - Fase 4

API .NET 8 para gerenciamento de usuarios, autenticacao JWT e publicacao de eventos do ecossistema Fase 4. A aplicacao roda como Web API containerizada, com suporte a Docker Compose, Kubernetes local e Amazon EKS.

## Visao geral

A Users API e o servico responsavel por:

- Cadastro, consulta, atualizacao e exclusao de usuarios.
- Login e emissao de tokens JWT.
- Persistencia em PostgreSQL via Entity Framework Core.
- Publicacao de eventos em RabbitMQ.
- Exposicao de health check em `/health`.
- Documentacao Swagger em ambiente de desenvolvimento.

## Arquitetura

- .NET 8 Web API executando via Kestrel na porta `8080`.
- PostgreSQL como banco relacional da API.
- RabbitMQ como broker de eventos compartilhado entre os microsservicos.
- JWT Bearer para autenticacao e autorizacao.
- Dockerfile e Docker Compose para execucao local.
- Manifests Kubernetes para ambiente local e Amazon EKS.
- HPA, Ingress e PVC para o deploy em EKS.
- Terraform para infraestrutura AWS do projeto.
- GitHub Actions para testes, build e push da imagem Docker.

## Configuracoes principais

Variaveis esperadas pela aplicacao:

- `ConnectionStrings__DefaultConnection`
- `Jwt__Secret`
- `Jwt__Issuer`
- `Jwt__Audience`
- `Jwt__KeyId`
- `Jwt__ExpirationHours`
- `RabbitMq__Host`
- `RabbitMq__Port`
- `RabbitMq__Username`
- `RabbitMq__Password`
- `RabbitMq__VirtualHost`
- `RabbitMq__ExchangeName`

Variaveis usadas pelos scripts e Docker Compose:

- `POSTGRES_PASSWORD`
- `USERS_DB_CONNECTION_STRING`
- `JWT_SECRET`
- `JWT_ISSUER`
- `JWT_AUDIENCE`
- `JWT_KEY_ID`
- `RABBITMQ_USERNAME`
- `RABBITMQ_PASSWORD`
- `RABBITMQ_VHOST`

## Execucao local com Docker Compose

```powershell
docker compose up --build
```

Servicos locais:

- Users API: `http://localhost:5000`
- Swagger: `http://localhost:5000`
- PostgreSQL: `localhost:5432`
- RabbitMQ AMQP: `localhost:5672`
- RabbitMQ Management: `http://localhost:15672`

O Compose sobe PostgreSQL, RabbitMQ e Users API na rede `fiap-ms-network`, usando vhost RabbitMQ `fiap` e exchange `fiap.events`.

## Execucao local com Kubernetes

```powershell
.\deployLocal.ps1
```

Ou manualmente:

```powershell
kubectl apply -f k8s/local
kubectl rollout status deployment/users-api -n fase4
```

Para acessar localmente:

```powershell
kubectl port-forward svc/users-api 8080:80 -n fase4
```

Depois acesse:

- API/Swagger: `http://localhost:8080`
- Health check: `http://localhost:8080/health`

## Infraestrutura AWS

```powershell
.\criarClusterEks.ps1
```

Ou via Terraform:

```powershell
cd iac/terraform/aws
Copy-Item terraform.tfvars.example terraform.tfvars
terraform init
terraform plan
terraform apply
```

## Deploy no EKS

```powershell
.\deployEks.ps1
```

O script conecta no cluster `fase4-users-cluster`, cria/atualiza secrets Kubernetes e aplica os manifests em `k8s/eks`.

Imagem usada nos manifests:

```text
adinteltidev/fase4-users-api:latest
```

## CI/CD

Workflow:

- `.github/workflows/docker-build-push.yml`: restaura dependencias, executa testes da solucao `ms-users.sln`, cria a imagem Docker e publica no Docker Hub.

Secrets esperados:

- `DOCKER_HUB_USERNAME`
- `DOCKER_HUB_TOKEN`
- `DOCKER_HUB_REPOSITORY`

## Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- RabbitMQ
- JWT Bearer
- Swagger
- Docker
- Kubernetes
- Amazon EKS
- Terraform
- xUnit
