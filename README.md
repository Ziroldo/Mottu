# Delivery Rental API

API para gestão de motos, entregadores e locações.

## Stack
- .NET 9 (ASP.NET Core)
- PostgreSQL
- RabbitMQ (management)
- MinIO (S3)
- EF Core

## Requisitos
- Docker + Docker Compose
- .NET SDK 9
- PowerShell 5.1+ ou 7+

## Infra
```powershell
docker compose up -d


ConnectionStrings: Host=localhost;Database=mottu;Username=postgres;Password=postgres
RabbitMq: Host=localhost;Port=5672;User=guest;Password=guest;ExchangeMotoCreated=mottu.moto.created
MinIO: Endpoint=localhost:9000;AccessKey=minioadmin;SecretKey=minioadmin;BucketName=mottu-cnh-bucket
App: http://localhost:5035

Swagger: http://localhost:5035/swagger

Endpoints
Motos

GET /api/Motos

GET /api/Motos/{id}

POST /api/Motos → { "placa": "ABC1D23" }

PUT /api/Motos/{id} → { "placa": "XYZ9Z99" }

DELETE /api/Motos/{id}

Regra: placa única.

Entregadores

PUT /api/Deliverers/{id}/cnh — multipart (file), PNG/BMP.
Armazenamento no MinIO e persistência do nome do objeto.

Locações

POST /api/Rentals → corpo mínimo:

{ "delivererId": 1, "motoId": 3, "plan": 7 }


Regras:

Início = dia seguinte à criação.

Planos: 7 (R$30/dia), 15 (R$28), 30 (R$22), 45 (R$20), 50 (R$18).

Apenas CNH A ou A+B.

GET /api/Rentals/{id}

PUT /api/Rentals/{id}/return → corpo é string ISO UTC, ex.: "2025-10-19T12:00:00Z"

Multas:

Devolução antecipada: multa sobre dias não usados

7 dias: 20%

15 dias: 40%

Atraso: R$50 por diária extra.

Teste rápido (PowerShell)
$api="http://localhost:5035"; $h=@{"Content-Type"="application/json"}

# Criar moto
Invoke-RestMethod -Method Post -Uri "$api/api/Motos" -Headers $h -Body '{ "placa":"BBB1C23" }'

# Upload CNH (id=1)
Add-Type -AssemblyName System.Net.Http
$cli=[Net.Http.HttpClient]::new()
$mp=[Net.Http.MultipartFormDataContent]::new()
$png="$env:TEMP\dummy-cnh.png"
[IO.File]::WriteAllBytes($png,[Convert]::FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHuALr1w8pYQAAAABJRU5ErkJggg=="))
$fs=[IO.File]::OpenRead($png)
$sc=[Net.Http.StreamContent]::new($fs)
$sc.Headers.ContentType=[Net.Http.Headers.MediaTypeHeaderValue]::Parse("image/png")
$mp.Add($sc,"file",[IO.Path]::GetFileName($png))
$cli.PutAsync("$api/api/Deliverers/1/cnh",$mp).Result.EnsureSuccessStatusCode() | Out-Null
$fs.Dispose(); $cli.Dispose()

# Criar e devolver locação
$rental = Invoke-RestMethod -Method Post -Uri "$api/api/Rentals" -Headers $h -Body '{"delivererId":1,"motoId":3,"plan":7}'
$rentalId=$rental.id
$bodyReturn='"'+([DateTime]::UtcNow.ToString("s")+'Z')+'"'
Invoke-RestMethod -Method Put -Uri "$api/api/Rentals/$rentalId/return" -Headers $h -Body $bodyReturn

Consoles

RabbitMQ: http://localhost:15672
 (guest/guest)

MinIO: http://localhost:9001
 (minioadmin/minioadmin)

Troubleshooting

Portas ocupadas → ajuste mapeamentos no compose.

409 em placas → violação de índice único.

400 em Rentals → verifique plan, delivererId, motoId e CNH.

Upload CNH → apenas PNG/BMP; bucket é criado automaticamente.
