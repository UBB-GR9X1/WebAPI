![image](https://github.com/user-attachments/assets/19f06456-5861-4385-9346-56b81fcc62a3)

# Project structure
### Please read the pdf before starting to code.
```
📁 ClassLibrary
│   ├── 📄 ClassLibrary.csproj
│   ├── 🧩 Domain            # Domain models (use this in WinUI project, do not redefine them there)
│   ├── 🧾 IRepository       # Interfaces (must be implemented in WinUI/Repository)
│   └── 📘 README.md
📄 Project setup - ISS.pdf
📄 UBB-SE-SLN.sln
📁 WebAPI
│   ├── 🎮 Controller
│   ├── 🗄️ Data
│   ├── 🧩 Entity
│   ├── 📜 LICENSE
│   ├── 📄 Pages
│   ├── 🧠 Program.cs
│   ├── ⚙️ Properties
│   ├── 📘 README.md
│   ├── 🛠️ Repository
│   ├── 📄 WebApi.csproj
│   ├── ⚙️ appsettings.Development.json
│   ├── ⚙️ appsettings.json
│   └── 🌐 wwwroot
📁 WinUI
│   ├── 📄 App.xaml
│   ├── 🧠 App.xaml.cs
│   ├── 🖼️ Assets
│   ├── 🪟 MainWindow.xaml
│   ├── 🧠 MainWindow.xaml.cs
│   ├── 🧩 Model
│   ├── 📦 Package.appxmanifest
│   ├── ⚙️ Properties
│   ├── 🛠️ Repository        # Implement interfaces from ClassLibrary/IRepository here
│   ├── 🔧 Service
│   ├── 🖼️ View
│   ├── 🧠 ViewModel
│   ├── 📄 WinUI.csproj
│   ├── ⚙️ WinUI.csproj.user
│   └── ⚙️ app.manifest
```

---

# ⚙️ In order to update your database, run the following:

> ❗️ This assumes that you dropped your local database first!
> ❗️ Edit: Osaki here, use the sql script, migrations are not working.

1. Go into the WebAPI folder:
```bash
cd WebAPI
```

2. Run the table creations script:
```bash
dotnet ef database update InitialCreate
```

3. Run the mock data seeding script:
```bash
dotnet ef database update SeedMockData
```
![Untitled555](https://github.com/user-attachments/assets/f43a058d-76c3-43cd-9e40-690882fa2599)

