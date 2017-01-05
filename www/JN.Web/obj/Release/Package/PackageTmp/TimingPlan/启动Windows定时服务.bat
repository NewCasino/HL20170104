%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe D:\AWP160817\WP160817\www\JN.Web\TimingPlan\JN.WindowsService.exe
Net Start WP160817
sc config WP160817 start= auto
pause