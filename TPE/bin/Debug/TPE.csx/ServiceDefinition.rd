<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="TPE" generation="1" functional="0" release="0" Id="0a748c95-eeef-4b63-a9ce-e9b927a90033" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="TPEGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WebRole1:HttpIn" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/TPE/TPEGroup/LB:WebRole1:HttpIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="WebRole1Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/TPE/TPEGroup/MapWebRole1Instances" />
          </maps>
        </aCS>
        <aCS name="WebRole1:DiagnosticsConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/TPE/TPEGroup/MapWebRole1:DiagnosticsConnectionString" />
          </maps>
        </aCS>
        <aCS name="WebRole1:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/TPE/TPEGroup/MapWebRole1:DataConnectionString" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:WebRole1:HttpIn">
          <toPorts>
            <inPortMoniker name="/TPE/TPEGroup/WebRole1/HttpIn" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapWebRole1Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/TPE/TPEGroup/WebRole1Instances" />
          </setting>
        </map>
        <map name="MapWebRole1:DiagnosticsConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/TPE/TPEGroup/WebRole1/DiagnosticsConnectionString" />
          </setting>
        </map>
        <map name="MapWebRole1:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/TPE/TPEGroup/WebRole1/DataConnectionString" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WebRole1" generation="1" functional="0" release="0" software="C:\WAPTK\Labs\IntroductionToWindowsAzureVS2010\Source\Ex1-BuildingYourFirstWindowsAzureApp\CS\TPE\TPE\bin\Debug\TPE.csx\roles\WebRole1" entryPoint="base\x64\WaWebHost.exe" parameters="" memIndex="1792" hostingEnvironment="frontendfulltrust" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="HttpIn" protocol="http" />
            </componentports>
            <settings>
              <aCS name="DiagnosticsConnectionString" defaultValue="" />
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WebRole1&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WebRole1&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/TPE/TPEGroup/WebRole1Instances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="WebRole1Instances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="8c6236ce-9dae-4aee-a9d9-ebca5d721ecb" ref="Microsoft.RedDog.Contract\ServiceContract\TPEContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="4a240e8c-b651-42da-bf96-8bb05ba490e4" ref="Microsoft.RedDog.Contract\Interface\WebRole1:HttpIn@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/TPE/TPEGroup/WebRole1:HttpIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>