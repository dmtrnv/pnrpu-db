using System.Collections.Generic;

namespace BoatStation.Viewmodel
{
    public static class RolesNavBar
    {
        private static readonly Dictionary<string, string> Data = new()
        {
            ["admin"] = 
                @"<div class=""navbar-collapse collapse d-sm-inline-flex justify-content-between"">
                    <ul class=""navbar-nav flex-grow-1"">
                        <li class=""nav-item"">
                            <a class=""nav-link text-dark"" href=""https://localhost:5001/Admin/AnyQuery"">Выполнить запрос</a>
                        </li>
                    </ul>
                 </div>",
            
            ["supplier"] =
                @"<div class=""navbar-collapse collapse d-sm-inline-flex justify-content-between"">
                    <ul class=""navbar-nav flex-grow-1"">
                        <li class=""nav-item"">
                            <a class=""nav-link text-dark"" href=""https://localhost:5001/Supply/OrdersRequests"">Заявки на заказ</a>
                        </li>
                        <li class=""nav-item"">
                            <a class=""nav-link text-dark"" href=""https://localhost:5001/Supply/Orders"">Заказы</a>
                        </li>
                    </ul>
                 </div>",
            
            ["repair"] = 
                @"<div class=""navbar-collapse collapse d-sm-inline-flex justify-content-between"">
                <ul class=""navbar-nav flex-grow-1"">
                    <li class=""nav-item"">
                        <a class=""nav-link text-dark"" href=""https://localhost:5001/Repair/RepairRequests"">Заявки на ремонт</a>
                    </li>
                </ul>
            </div>",
            
            ["reception"] = 
                @"<div class=""navbar-collapse collapse d-sm-inline-flex justify-content-between"">
                    <ul class=""navbar-nav flex-grow-1"">
                        <li class=""nav-item"">
                            <a class=""nav-link text-dark"" href=""https://localhost:5001/Reception/CreateContract"">Выдать<br>плав. ср.</a>
                        </li>
                        <li class=""nav-item"">
                            <a class=""nav-link text-dark"" href=""https://localhost:5001/Reception/ActiveContracts"">Выданные<br>плав. ср.</a>
                        </li>
                         <li class=""nav-item"">
                            <a class=""nav-link text-dark"" href=""https://localhost:5001/Reception/AvailableWatercrafts"">Имеющиеся<br>плав. ср.</a>
                        </li>
                         <li class=""nav-item"">
                            <a class=""nav-link text-dark"" href=""https://localhost:5001/Reception/AvailablePonds"">Существующие<br>водоемы</a>
                        </li>
                        <li class=""nav-item"">
                            <a class=""nav-link text-dark"" href=""https://localhost:5001/Reception/ContractArchive"">История выдачи<br>плав. ср.</a>
                        </li>
                        <li class=""nav-item"">
                            <a class=""nav-link text-dark"" href=""https://localhost:5001/Reception/ClientsBase"">База<br>клиентов</a>
                        </li>
                    </ul>
                 </div>"
        };
        
        public static string GetNavBar(string role)
        {
            if (role is null || !Data.ContainsKey(role))
            {
                return @"<div class=""navbar-collapse collapse d-sm-inline-flex justify-content-between""></div>";
            }
                
            return Data[role];
        }
    }
}