# SOS Emergency Service - Technical Report

## Question 1a: Geospatial & Connectivity Implementation (5 marks)

The application implements robust location and connectivity tracking logic ensuring critical data is available during emergencies.

**1. Automatic Geolocation Retrieval**
- **Implementation**: The `MainPage.OnAppearing()` method automatically triggers `GetGeolocationAsync()`.
- **API Usage**: Utilizes `Microsoft.Maui.Devices.Sensors` namespace.
- **Logic**:
  - Uses `GeolocationRequest` with `GeolocationAccuracy.Medium` (10s timeout) to balance speed and accuracy.
  - Fail-safe logic: Wraps calls in `try-catch` blocks to prevent crashes if GPS is disabled, displaying "Unavailable" or error messages instead.
  - **Code Ref**: `MainPage.xaml.cs` lines 540-590.

**2. Network Connectivity Monitoring**
- **Automatic Check**: `CheckConnectivity()` is called immediately on launch.
- **Dynamic Monitoring**: Subscribes to `Connectivity.Current.ConnectivityChanged` event to update the UI in real-time if the user loses/regains internet.
- **UI Feedback**: Displays a dynamic status icon (🟢/🔴) in the top header without obstructing the user flow.
- **Code Ref**: `MainPage.xaml.cs` lines 592-614.

**Step-by-Step Process:**

1. **Namespace Import**: The code imports `Microsoft.Maui.Devices.Sensors` which provides access to device sensors including GPS.
   ```csharp
   using Microsoft.Maui.Devices.Sensors;
   ```

2. **Trigger on Page Load**: When `MainPage` appears, `OnAppearing()` is called automatically. This method initiates the geolocation fetch:
   ```csharp
   protected override async void OnAppearing()
   {
       await GetGeolocationAsync();
       CheckConnectivity();
   }
   ```

3. **Geolocation Request**: The `GetGeolocationAsync()` method creates a request with medium accuracy and 10-second timeout:
   ```csharp
   var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
   var location = await Geolocation.Default.GetLocationAsync(request);
   ```

4. **Data Extraction**: Once retrieved, latitude and longitude are stored as formatted strings:
   ```csharp
   _currentLatitude = location.Latitude.ToString("F6");
   _currentLongitude = location.Longitude.ToString("F6");
   ```

### 2. Network Connectivity Monitoring

**Step-by-Step Process:**

1. **Initial Check**: On page load, `CheckConnectivity()` reads the current network state:
   ```csharp
   UpdateConnectivityUI(Connectivity.Current.NetworkAccess);
   ```

2. **Dynamic Subscription**: The app subscribes to connectivity changes for real-time updates:
   ```csharp
   Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
   ```

3. **UI Update Logic**: The `UpdateConnectivityUI()` method updates the status icon based on `NetworkAccess` enum:
   ```csharp
   switch (networkAccess)
   {
       case NetworkAccess.Internet:
           NetworkStatusLabel.Text = "🟢";  // Green = Online
           break;
       case NetworkAccess.None:
           NetworkStatusLabel.Text = "🔴";  // Red = Offline
           break;
   }
   ```

---

## Question 1b: Responsive UI Development (10 marks)

### 1. Primary Grid Structure
- The `MainPage` uses a `Grid` as the root layout container, allowing for precise row definitions (`RowDefinitions`) that adapt to screen height.
- **Code Ref**: `MainPage.xaml` lines 6-20.

**2. Visual Appeal & Responsiveness**
- **FlexLayout**: The Category selection uses `FlexLayout` with `Wrap="Wrap"` and `JustifyContent="SpaceEvenly"`. This ensures buttons flow naturally on different screen widths (e.g., 3-per-row on phone vs. 6-per-row on tablet) without horizontal scrolling.
- **Theme**: Consistent "Coral" (#FF6B4A) primary color scheme used across Frames, Buttons, and Icons.
- **Professional Elements**: Uses shadowed `Frame` elements with rounded corners (`CornerRadius`), opacity overlays for popups, and micro-animations (scale/pulse) on button interactions.

**Step-by-Step Process:**

1. **Root Layout**: The `MainPage.xaml` uses `Grid` as the primary container:
   ```xml
   <Grid Padding="20" RowSpacing="15">
       <Grid.RowDefinitions>
           <RowDefinition Height="Auto" />  <!-- Row 0: Header -->
           <RowDefinition Height="Auto" />  <!-- Row 1: Title -->
           <!-- ... more rows ... -->
       </Grid.RowDefinitions>
   </Grid>
   ```

2. **Row Assignment**: Each UI section is placed in a specific row using `Grid.Row`:
   ```xml
   <Frame Grid.Row="6">  <!-- Geolocation Display -->
       <Label x:Name="LatitudeLabel" Text="Loading..." />
       <Label x:Name="LongitudeLabel" Text="Loading..." />
   </Frame>
   ```

### 2. Visual Appeal & Responsiveness

**FlexLayout for Categories:**
```xml
<FlexLayout Wrap="Wrap" JustifyContent="SpaceEvenly">
    <Frame x:Name="CategoryMedical">🏥 Medical</Frame>
    <Frame x:Name="CategoryFire">🔥 Fire</Frame>
    <!-- Automatically wraps on smaller screens -->
</FlexLayout>
```

**Consistent Theme:**
- Primary Color: Coral (#FF6B4A)
- Background: Warm white (#FFF8F5)
- All interactive elements use matching coral-tinted borders and shadows.

---

## Question 2a: SQLite Integration Steps (5 marks)

The application follows the 4-step integration process for SQLite as strictly defined in the requirements (Microsoft, 2024).

**Step 1: Package Installation & Reference**
- **NuGet Packages**: `sqlite-net-pcl` and `SQLitePCLRaw.bundle_green` are added to the project.
- **Ref**: `SOS Emergency Service.csproj` lines 66-67.

```xml
<PackageReference Include="sqlite-net-pcl" Version="1.9.172" />
<PackageReference Include="SQLitePCLRaw.bundle_green" Version="2.1.11" />
```

**Step 2: Model Definition**
- **Model**: `Incident.cs` class implemented with `[PrimaryKey]` attribute (Kruger, 2023).
- **Data Integrity**: Stores `TripID` (string), `Latitude` (string), and `Longitude` (string) as required.

```csharp
public class Incident
{
    [PrimaryKey]
    public string? TripID { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
}
```

**Step 3: Connection Setup**
- **Implementation**: The `EmergencyDatabase` class instantiates `SQLiteAsyncConnection`.
- **Path**: Database stored in `FileSystem.AppDataDirectory` for secure local persistence.

```csharp
_database = new SQLiteAsyncConnection(DatabasePath, 
    SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);
```

**Step 4: Table Initialization**
- **Implementation**: `Init()` method is called before operations to ensure tables exist.

```csharp
await _database.CreateTableAsync<Incident>();
```

---

## Question 2b: Secure Offline Data Persistence and Validation (8 marks)

### Part (i): Why Synchronous Client-Side Validation is Important

**1. Immediate User Feedback**
Client-side validation executes instantly on the device without network latency (Mozilla, 2024). Users receive immediate feedback about input errors before attempting a database operation, improving the user experience in time-critical emergency scenarios.

**2. Data Integrity & Reliability**
By validating data before it reaches the database layer, we prevent malformed or invalid data from being written to SQLite (Microsoft, 2024). This maintains consistency and reduces the overhead of handling database constraint violations at runtime.

### Part (ii): Validation Rules and Visual Feedback Mechanism

#### Validation Rules for Trip ID
The Trip ID field satisfies the following enterprise requirements:

| Rule | Description | Logic Used |
|------|-------------|------------|
| **Non-Empty** | Field cannot be blank | `!string.IsNullOrWhiteSpace()` |
| **Exact Length** | Must be exactly 8 characters | `tripId.Length == 8` |
| **Alphanumeric** | Only A-Z, 0-9 allowed | Regex: `^[a-zA-Z0-9]+$` |

**Step-by-Step Validation Process:**

1. **Text Change Event**: When the user types, `OnTripIdTextChanged` fires:
   ```csharp
   private void OnTripIdTextChanged(object sender, TextChangedEventArgs e)
   {
       ValidateTripId(e.NewTextValue, showError: false);
   }
   ```

2. **Validation Logic**: The `ValidateTripId()` method checks all rules:
   ```csharp
   if (tripId.Length != 8)
   {
       ShowValidationError($"Must be 8 characters (current: {tripId.Length})");
       return false;
   }
   if (!Regex.IsMatch(tripId, @"^[a-zA-Z0-9]+$"))
   {
       ShowValidationError("Only letters and numbers allowed");
       return false;
   }
   ```

3. **Visual Feedback**: Border color and error message update instantly:
   ```csharp
   TripIdFrame.Stroke = new SolidColorBrush(Color.FromArgb("#EF5350")); // Red
   ErrorLabel.Text = $"⚠️ {message}";
   ErrorFrame.IsVisible = true;
   ```

#### Visual Feedback Mechanism
The application provides immediate visual cues based on validation state:

| Validation State | Border Color | Error Message Display |
|------------------|--------------|----------------------|
| **Empty/Blank** | Red (#EF5350) | "Trip ID cannot be empty" |
| **Wrong Length** | Yellow (#f9c74f) | Warning mode (no text, just border) |
| **Invalid Char** | Red (#EF5350) | "Only letters and numbers allowed" |
| **Valid** | Coral (#FF6B4A) | Error label hidden, border matches theme |

---

## References

Kruger, F. (2023). *sqlite-net-pcl: Simple, powerful, cross-platform SQLite client* [Computer software]. GitHub. https://github.com/praeclarum/sqlite-net

Microsoft. (2024). *Geolocation in .NET MAUI*. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device/geolocation

Microsoft. (2024). *Local databases in .NET MAUI*. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/database-local

Mozilla. (2024). *Client-side form validation*. MDN Web Docs. https://developer.mozilla.org/en-US/docs/Learn/Forms/Form_validation

---

## AI Declaration

### Tool 1: Google Gemini (Antigravity)
- I used **Google Gemini (Antigravity)** to assist with code implementation and debugging.
- The output was incorporated in **MainPage.xaml.cs** (geolocation logic, validation methods, share popup handlers) and **EmergencyDatabase.cs** (SQLite operations).
- I subsequently modified the output by reviewing all generated code, testing functionality on the Android emulator, adjusting UI styling to match the Coral theme, and removing unused legacy code to simplify the codebase.

### Tool 2: Google Gemini (Antigravity)
- I used **Google Gemini (Antigravity)** to assist with writing the Technical Report documentation.
- The output was incorporated in **TechnicalReport.md** (Q1a, Q1b, Q2a, Q2b explanations and code snippets).
- I subsequently modified the output by verifying all code references match the actual implementation, adjusting line number references, and formatting content to meet assessment requirements.
