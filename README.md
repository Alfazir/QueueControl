# QueueControlServer-1.0.1.2
 Код небольшого тестового проекта, для программно-аппаратного комплекса электронных очередей.  Разработка велась на ASP.NET Core 5, c последующей миграцией на  .Net 6 
  Краткое описание: 
  Проект представляет из себя веб приложение, - сервер для управления терминалами электронных очередей.
  
Цели и задачи проекта: 

Проект создавался для контроля очередей на заводах. Заводы отгружают перевозчикам грузы. При регистрации в очереди, указывается марка груза и вид упаковки, эти данные определяют, в какую очередь будет помещён регистрируемый. 
Регистрация может осуществляться посредством сканирования карты представителя перевозчика. Тогда данные об очереди получаются из учётной системы перевозчика, интегрированного с сервером контроля очередей.   

    Реализовано:  
 - сервер предоставляет вею-интерфейс для киосков, дающий возможность регистрации в очереди путём сканирования карты, либо ввода данных о регистрируемом вручную.
- веб интерфейс диспетчера для отслеживания текущего состояния очередей
- возможность создавать новые очереди/ учётные записи для киосков/диспетчеров. 
- API для управления очередями. Получения информации об очередях и интеграции с сторонними учётными системами перевозчика. 
P.S.  для запуска проекта  предварительно переименуйте файл Exampleappsettings.json в appsettings.json и измените в файле строку "DefaultConnection". 
Блок MySettings содерджит параметры для интеграции с учётной системой перевозчика.

Аутентификационные  данные админской уч. записи:
admin@gmail.com
Admin_1234


Проект только для демонстрации и не на что не претендует! 


 
