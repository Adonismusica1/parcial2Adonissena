#!/usr/bin/env pwsh
<#
.SYNOPSIS
Script para ejecutar la API de ComunidadApp

.DESCRIPTION
Ejecuta la API REST en modo Development en http://localhost:5000

.EXAMPLE
./run-api.ps1
#>

Write-Host "===================================" -ForegroundColor Cyan
Write-Host "Iniciando ComunidadApp API" -ForegroundColor Green
Write-Host "===================================" -ForegroundColor Cyan
Write-Host ""

# Verificar que .NET está instalado
Write-Host "Verificando .NET..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version
Write-Host "✓ .NET $dotnetVersion encontrado" -ForegroundColor Green
Write-Host ""

# Restaurar dependencias
Write-Host "Restaurando dependencias..." -ForegroundColor Yellow
dotnet restore | Out-Null
Write-Host "✓ Dependencias restauradas" -ForegroundColor Green
Write-Host ""

# Compilar
Write-Host "Compilando proyecto..." -ForegroundColor Yellow
dotnet build -c Release -q
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Compilación exitosa" -ForegroundColor Green
}
else {
    Write-Host "✗ Error durante la compilación" -ForegroundColor Red
    exit 1
}
Write-Host ""

# Ejecutar
Write-Host "===================================" -ForegroundColor Cyan
Write-Host "Iniciando servidor..." -ForegroundColor Green
Write-Host "===================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "La API estará disponible en:" -ForegroundColor Yellow
Write-Host "  URL Base: http://localhost:5000" -ForegroundColor Green
Write-Host "  Swagger:  http://localhost:5000/swagger" -ForegroundColor Green
Write-Host ""
Write-Host "Presione Ctrl+C para detener el servidor" -ForegroundColor Yellow
Write-Host ""

$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run
