## Backend (.NET)

### Prerequisites
- .NET 8 or later installed
- SQL Server or any required database setup

### Backend (C# .Net Core)
1. Navigate to the backend project directory:
   ```bash
   cd noted-api
2. Restore dependencies:
   ```bash
   dotnet restore
3. Apply migrations (if using EF Core):
   ```bash
   dotnet ef database update
4. Run the application:
   ```bash
   dotnet run

5. The API will be available at \`https://localhost:5000\` (or another configured port).

## Frontend (Vue.js)

### Prerequisites
- Node.js (LTS version) installed
- Vue CLI installed globally (\`npm install -g @vue/cli\`)

### Running the Frontend
1. Navigate to the frontend project directory:
   ```bash
   cd noted-app
2. Install dependencies:
   ```bash
   npm install
3. Start the development server:
   ```bash
   npm run dev
   
4. The frontend will be available at \`http://localhost:5173\` (or another configured port).

