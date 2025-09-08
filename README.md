# SnapToTable 🍳📱

A full-stack application that uses AI to extract recipe information from food images. Built with React Native (Expo) for the mobile frontend and .NET 9 for the backend API.

> **Portfolio Project** 🎯 This project was created as part of my learning journey with **React Native** and **Clean Architecture** principles. It serves as a practical demonstration of modern mobile development practices, clean code organization, and full-stack application design.

## 🎬 Demo

### Quick Preview
![SnapToTable Demo](https://raw.githubusercontent.com/pilucdes/PublicMedia/refs/heads/main/snap-to-table/snap-to-table-showcase.gif)

### Full Demo Video
📱 **[Watch Full Demo Video](https://youtu.be/UFSeged0hVY)**

---

## 🚀 Features

- **AI-Powered Recipe Extraction**: Upload food images and automatically extract recipe details
- **Cross-Platform Mobile App**: Built with React Native and Expo for Web, iOS and Android
- **Modern Backend**: Clean Architecture with .NET 8, CQRS, and MongoDB
- **Recipe Management**: Browse and search extracted recipes
- **Responsive UI**: Beautiful, theme-aware interface with Tailwind CSS

## 🏗️ Architecture

### Frontend (Mobile App)
- **Framework**: React Native with Expo
- **Navigation**: Expo Router with file-based routing
- **State Management**: React Query for server state
- **Styling**: Tailwind CSS with custom theme system
- **UI Components**: Custom themed components with animations

### Backend (API)
- **Framework**: .NET 9 Web API
- **Architecture**: Clean Architecture with CQRS pattern
- **Database**: MongoDB with repository pattern
- **AI Integration**: OpenAI API for recipe extraction
- **Validation**: FluentValidation with behavior pipeline
- **Documentation**: OpenAPI with Scalar

## 📱 Mobile App Structure

```
client/snap-to-table/
├── app/                    # Expo Router pages
├── features/              # Feature-based organization
│   ├── recipes/          # Recipe management
│   └── common/           # Shared components
├── themes/               # Theme system
├── lib/                  # Utilities and configurations
└── assets/               # Images and static files
```

## 🔧 Backend Structure

```
src/
├── SnapToTable.API/           # Web API layer
├── SnapToTable.Application/   # Business logic & CQRS
├── SnapToTable.Domain/        # Domain entities & contracts
└── SnapToTable.Infrastructure/ # Data access & external services
```

## 🚀 Getting Started

### Prerequisites
- Node.js 18+ and npm
- .NET 9 SDK
- MongoDB instance
- OpenAI API key
- Expo CLI (`npm install -g @expo/cli`)

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd src/SnapToTable.API
   ```

2. Configure your settings in `appsettings.json` or `user secrets`:
   ```json
   {
     "MongoDbSettings": {
       "ConnectionString": "your_mongodb_connection_string",
       "DatabaseName": "SnapToTable"
     },
     "OpenAiSettings": {
       "ApiKey": "your_openai_api_key"
     }
   }
   ```

3. Run the API:
   ```bash
   dotnet run
   ```

4. Access the API documentation at `http://localhost:5261/scalar`

### Frontend Setup

1. Navigate to the mobile app directory:
   ```bash
   cd client/snap-to-table
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npx expo start
   ```

4. Use Expo Go app or run on simulator/emulator

## 🐋 Docker Setup
1. Create an .env file containing the variable "OPENAI_API_KEY" and the key value e.g. :
   ```
   OPENAI_API_KEY=YOURKEY
   ```
2. Run docker compose 
   ```
   docker compose up
   ```
3. API documentation available at http://localhost:5261/scalar

4. Application available at http://localhost:8081

## 📋 API Endpoints

- `GET /api/v1/recipes` - Get all recipes with pagination
- `GET /api/v1/recipes/{id}` - Get recipe by ID
- `POST /api/v1/recipe-analysis` - Create recipe analysis from images

## 🧪 Testing

The project includes comprehensive testing:

- **Unit Tests**: For application logic and domain
- **Integration Tests**: For API endpoints and repositories
- **Test Projects**: Separate test projects for each layer

Run tests from the solution root:
```bash
dotnet test
```

## 🎨 UI Components

The mobile app includes a comprehensive component library:

- **ThemeAreaView**: Themed container components
- **ThemeButton**: Consistent button styling
- **ThemeText**: Typography components
- **ThemeTextInput**: Form input components
- **Animation Components**: Smooth transitions and effects

## 🔐 Configuration

### Environment Variables
- `MongoDb__ConnectionString`: Connection string
- `MongoDb__DatabaseName`: Database name
- `MongoDb__UseTls`: (default: true)
- `OpenAi__ApiKey`: API key for recipe extraction
- `OpenAi__Token`: Maximum number of tokens allowed for the generated answer (default:4096)
- `OpenAi__Model`: Model to use (default: gpt-4.1)
- `OpenAi__ImageModel`: Pmage gen. model to use (default: dall-e-3)
- `OpenAi__ImageSize`: Image gen. size (default: 1024x1024)

## 🚀 Future Improvements & Missing Features

### 🔐 **1. Authorization & User Management**
- **User Authentication**: Implement JWT-based authentication system
- **User Profiles**: Personal recipe collections and preferences
- **Role-Based Access**: Admin, premium, and basic user tiers
- **Social Features**: Share recipes, follow other users, and collaborative cooking

### 🧪 **2. Frontend Testing**
- **Unit Tests**: Component testing with React Testing Library
- **Integration Tests**: Screen and user flow testing

### ⚡ **3. Streaming & Async Processing**
- **Streaming Content**: Progressive loading of recipe data

### 🤖 **4. Specialized LLM Integration**
- **Domain-Specific Models**: Fine-tuned models for recipe extraction and image generation
