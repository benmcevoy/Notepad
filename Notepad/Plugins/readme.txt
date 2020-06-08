put plugins here

TODO: document, must implement Notepad.Command

Jinkies.  Create a .NET Core Library and edit the project to include     

<UseWPF>true</UseWPF>
<ExcludeAssets>runtime</ExcludeAssets>

Then reference Notepad.exe I guess.
Then copy your DLL into /plugins (here)

e.g.

<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <UseWPF>true</UseWPF>
    <ExcludeAssets>runtime</ExcludeAssets>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\Notepad\Notepad.csproj" />
  </ItemGroup>

</Project>
