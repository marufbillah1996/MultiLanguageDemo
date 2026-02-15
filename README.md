# MultiLanguageDemo - Optimized .NET 8 Application

A fully optimized multilingual demonstration application built with ASP.NET Core 8.0, Entity Framework Core, and modern best practices.

## 🚀 Optimization Features Implemented

### Performance Optimizations
- ✅ **Memory Caching**: Implemented in-memory caching for frequently accessed data (articles, categories)
- ✅ **Response Caching**: Added HTTP response caching with appropriate cache durations
- ✅ **Response Compression**: Brotli and Gzip compression enabled for all responses
- ✅ **Database Query Optimization**: 
  - AsNoTracking() for read-only queries
  - Filtered Include() to load only necessary translations
  - Split queries for better performance with multiple includes
  - Database indexes on frequently queried fields (Slug, IsActive, CategoryId, IsFeatured)
- ✅ **Connection Resiliency**: Automatic retry logic for database connections
- ✅ **Async View Count Updates**: Non-blocking view count increments

### API & Documentation
- ✅ **Swagger/OpenAPI**: Full API documentation available at `/api-docs`
- ✅ **Health Checks**: Endpoint at `/health` for monitoring application and database health
- ✅ **CORS Configuration**: Properly configured Cross-Origin Resource Sharing
- ✅ **JSON Serialization**: Optimized with camelCase naming policy

### Security Enhancements
- ✅ **Security Headers**: X-Content-Type-Options, X-Frame-Options, X-XSS-Protection, Referrer-Policy
- ✅ **Content Security Policy (CSP)**: Implemented for production environment
- ✅ **Cookie Security**: Secure, HttpOnly, SameSite cookies
- ✅ **HTTPS Enforcement**: Automatic HTTPS redirection

### Code Quality
- ✅ **Structured Logging**: Enhanced logging with context information
- ✅ **Environment-Specific Configuration**: Separate settings for Development/Production
- ✅ **EditorConfig**: Consistent code style across the project
- ✅ **Error Handling**: Proper exception handling with logging

### DevOps & Deployment
- ✅ **Docker Support**: Dockerfile and docker-compose.yml for containerization
- ✅ **Production-Ready**: Optimized build configuration
- ✅ **Static File Caching**: Long-term caching for static assets (1 year)

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server (2019 or later)
- Docker (optional, for containerized deployment)

## 🛠️ Installation & Setup

### Local Development

1. Clone the repository:
```bash
git clone https://github.com/marufbillah1996/MultiLanguageDemo.git
cd MultiLanguageDemo
```

2. Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=MultiLanguageDemoDB;..."
}
```

3. Apply database migrations:
```bash
cd MultiLanguageDemo
dotnet ef database update
```

4. Run the application:
```bash
dotnet run
```

5. Access the application:
   - Web App: https://localhost:5001
   - API Docs: https://localhost:5001/api-docs
   - Health Check: https://localhost:5001/health

### Docker Deployment

1. Build and run with Docker Compose:
```bash
docker-compose up -d
```

2. Apply migrations to the containerized database:
```bash
docker-compose exec web dotnet ef database update
```

3. Access the application:
   - Web App: http://localhost:5000
   - API Docs: http://localhost:5000/api-docs
   - Health Check: http://localhost:5000/health

## 🌐 Supported Languages

- English (en-US)
- Bengali (bn-BD)
- French (fr-FR)
- Arabic (ar-SA)
- Chinese (zh-CN)

## 📊 API Endpoints

### Articles
- `GET /api/ArticlesApi` - Get all articles
- `GET /api/ArticlesApi/{slug}` - Get article by slug
- `GET /api/ArticlesApi/featured?count=3` - Get featured articles
- `GET /api/ArticlesApi/categories` - Get all categories

All API endpoints support:
- Query parameter: `?culture=en-US`
- Header: `X-Language: en-US`

## 🔧 Configuration Options

### Cache Settings
Configure cache expiration in `appsettings.json`:
```json
"CacheSettings": {
  "DefaultExpirationMinutes": 15,
  "CategoryExpirationMinutes": 30
}
```

### CORS Settings
Configure allowed origins in `appsettings.json`:
```json
"AllowedOrigins": [
  "http://localhost:5000",
  "https://localhost:5001"
]
```

## 📈 Performance Monitoring

### Health Checks
Monitor application health:
```bash
curl https://localhost:5001/health
```

### Cache Performance
The application uses in-memory caching with the following strategies:
- Articles: 15 minutes TTL
- Categories: 30 minutes TTL (less frequently changed)
- Featured Articles: 15 minutes TTL

Cache is automatically invalidated when:
- Articles are viewed (view count updates)
- Data is modified

## 🔐 Security Best Practices

1. **Connection Strings**: Store sensitive data in environment variables or Azure Key Vault
2. **HTTPS**: Always use HTTPS in production
3. **Security Headers**: All recommended headers are enabled
4. **Input Validation**: Data annotations on models
5. **SQL Injection Protection**: Entity Framework Core with parameterized queries

## 📝 Additional Optimization Recommendations

### Future Enhancements
1. **Distributed Caching**: Implement Redis for multi-server scenarios
2. **CDN Integration**: Use Azure CDN or Cloudflare for static assets
3. **Database Query Store**: Enable SQL Server Query Store for query performance insights
4. **Application Insights**: Add Azure Application Insights for telemetry
5. **Rate Limiting**: Implement per-IP rate limiting for API endpoints
6. **Background Jobs**: Use Hangfire or Azure Functions for scheduled tasks
7. **Database Partitioning**: Partition large tables by date for better performance
8. **Read Replicas**: Use SQL Server read replicas for read-heavy workloads
9. **GraphQL**: Consider GraphQL for more flexible API queries
10. **Output Caching**: Implement output caching for frequently requested pages

### Code Quality Improvements
1. **Unit Tests**: Add comprehensive unit tests with xUnit
2. **Integration Tests**: Add integration tests for API endpoints
3. **Code Coverage**: Aim for >80% code coverage
4. **Static Analysis**: Use SonarQube or similar tools
5. **Performance Testing**: Use tools like k6 or JMeter

### Monitoring & Observability
1. **Structured Logging**: Already implemented, consider Serilog sinks
2. **APM Tools**: Application Performance Monitoring (New Relic, DataDog)
3. **Error Tracking**: Sentry or similar error tracking service
4. **Metrics**: Export custom metrics for business KPIs

## 🤝 Contributing

Contributions are welcome! Please follow these steps:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 👤 Author

**Maruf Billah**

---

⭐ If you find this project helpful, please give it a star!