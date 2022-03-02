NCrunch configuration tips for NFluent contributors
===================================================

Step1: Configure NCrunch to execute only the (.NET) 4.5 subset of NFluent
---------------------------------------------------------------------
For the record, NFluent.45 = NFluent.35 checks (*) + async/await checks + TPL checks 

To do so:
Within the 'NCrunch Configuration' tab, set 'true' for the option: General->'Ignore this component completely' for all projects BUT NFluent.45, NFluent.45.Tests, NFluent.Tests.Extensions.45


(*): The NFluent.45 project is linked to the NFluent.35 one through the 'Project Linker 2012' Visual Studio extension.
