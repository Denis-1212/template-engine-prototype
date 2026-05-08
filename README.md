<img width="917" height="799" alt="изображение" src="https://github.com/user-attachments/assets/49fda55a-e4ef-4a4c-96fe-e66852430700" />


## О проекте

Template Engine Prototype представляет собой веб-приложение для обработки текстовых шаблонов с подстановкой персональных данных (ФИО). Поддерживает два режима обработки:

- **Explicit Mode** - явное склонение по падежам с использованием предопределенных правил
- **AI Mode** - обработка с использованием qwen2.5:7b

### Основные возможности

- Подстановка имени, фамилии и отчества в шаблон
- Поддержка различных падежей (именительный, родительный, дательный и т.д.)
- Два режима обработки на выбор
- Мгновенный предпросмотр результата

## Технологии

### Бэкенд
- .NET 10
- ASP.NET Core Web API
- C# 14

### Фронтенд
- React 19
- TypeScript
- CSS Modules
- Fetch API

## Архитектура
```
Client (React) → Template Controller 
                     ↳ Explicit Processor
                     ↳ AI Processor
                                   → Response
```
## AI Processor Requirements
Для работы AI режима требуется локально запущенная модель через Ollama.

Настройка:
```
{
  "Ollama": {
    "Endpoint": "http://localhost:11434",
    "ModelName": "qwen2.5:7b",
    "TimeoutSeconds": 120,
    "MaxTokens": 200,
    "Temperature": 0.8
  }
}
```
