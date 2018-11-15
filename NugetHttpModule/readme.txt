  <system.webServer>
    <!-- IIS7.0集成模式下 -->
    <modules runAllManagedModulesForAllRequests="true">
      <add name="CheckPlatform" type="NugetHttpModule.CheckPlatformHttpModule,NugetHttpModule"></add>
    </modules>
  </system.webServer>

  <!-- IIS7.0经典模式下 -->
  <httpModules>
    <add name="CheckPlatform" type="NugetHttpModule.CheckPlatformHttpModule,NugetHttpModule"></add>
  </httpModules>