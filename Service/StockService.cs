using AutoMapper.Features;
using ExcelDataReader;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Stock_CMS.Service
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _StockRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDocRepository _docRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ITrackingRepository _trackingRepository;

        public StockService(IStockRepository StockRepository, IUserRepository userRepository, ICustomerRepository customerRepository, IDocRepository docRepository, ICompanyRepository companyRepository, ITrackingRepository trackingRepository)
        {
            _StockRepository = StockRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _docRepository = docRepository;
            _companyRepository = companyRepository;
            _trackingRepository = trackingRepository;
        }

        public async Task<long> AddStock(StockDto data)
        {
            //var isExist = await _StockRepository.GetStockByInfo(data);
            //if (isExist.Any()) { return -1; }
            //else
            //{
                List<StockDto> dataList = new List<StockDto> { data };
                var result = await _StockRepository.AddStock(dataList);
                if (result.Any())
                {
                    return result.FirstOrDefault().Id;
                }
                else
                {
                    return 0;
                }
            //}
        }
        public async Task<Int32> UpdateStock(StockDto data)
        {
            var isExist = await _StockRepository.GetStockById(data.Id);
            //var chk = await _StockRepository.GetStockByName(data.StockName);
            //bool isMatch = chk.Any(x => x.StockName.ToLower() == data.StockName.ToLower() && x.Id != data.Id);
            //if (isMatch)
            //{
            //    return -1;
            //}

            if (isExist.Id > 0)
            {  
                    data.CreatedBy = isExist.CreatedBy;
                    data.CreatedAt = isExist.CreatedAt;
                    data.UpdatedBy = data.UpdatedBy;
                    data.UpdatedAt = DateTime.Now;
                data.IsActive = isExist.IsActive;

                List<StockDto> updateList = new List<StockDto> { data };
                var result = await _StockRepository.UpdateStock(updateList);
                if (result.Any())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -2;
            }

        }

        public async Task<Int32> UpdateStockJson(long id, string jsonString, int updatedBy)
        {
            var isExist = await _StockRepository.GetStockById(id);
            var data = isExist;

            if (isExist.Id > 0)
            {
                data.StockJson = jsonString;
                data.UpdatedBy = updatedBy;
                data.UpdatedAt = DateTime.Now;

                List<StockDto> updateList = new List<StockDto> { data };
                var result = await _StockRepository.UpdateStock(updateList);
                if (result.Any())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -2;
            }

        }
        public async Task<IEnumerable<StockDto>> GetStockByClientId(long  clientid)
        {
            var data = await _StockRepository.GetStockByClientId(clientid);

            var ids = data.Select(x => x.CreatedBy).Concat(data.Select(x => x.UpdatedBy)).Distinct().ToArray();
            var users = await _userRepository.GetUsersByIds(ids);
            var result = data.Select(x =>
			{
				x.CreatedByName = users.FirstOrDefault(u => u.Id == x.CreatedBy)?.Name;
				x.UpdatedByName = users.FirstOrDefault(u => u.Id == x.UpdatedBy)?.Name;
				return x;
			});

            //var companyid = result.Select(x => x?.CompanyId).Distinct().ToArray();
            //var company =  await _companyRepository.GetCompanyByIds(companyid);
            //var results = result.Select(x =>
            //{
            //    x.CompanyName1 = company.FirstOrDefault(u => u.Id == x.CompanyId)?.CompanyName;
            //    return x;
            //});

            //var holdersids = results.Select(x => x?.FirstHolderId).Concat(data.Select(x => x?.SecondHolderId)).Concat(data.Select(x => x?.ThirdHolderId)).Distinct().ToArray();
            //var holders = await _docRepository.GetDocByIds(holdersids);
            //var result1 = results.Select(x =>
            //{
            //    x.FirstHolder1 = holders.FirstOrDefault(u => u.Id == x.FirstHolderId)?.Name;
            //    x.SecondHolder1 = holders.FirstOrDefault(u => u.Id == x.SecondHolderId)?.Name;
            //    x.ThirdHolder1 = holders.FirstOrDefault(u => u.Id == x.ThirdHolderId)?.Name;
            //    return x;
            //});

            return result;
        }

		public async Task<StockDto> GetStockById(long id)
		{  
			var result = await _StockRepository.GetStockById(id);
			var customer = await _customerRepository.GetCustomerById(result.CustomerId ?? 0);
            result.Customer = customer;
            var compony = await _companyRepository.GetCompanyById(result.CompanyId ?? 0);
            result.Company = compony.FirstOrDefault();
            var holder1 = await _docRepository.GetDocById(result.FirstHolderId ?? 0);
            result.FirstHolderData = holder1.FirstOrDefault(); 
            var holder2 = await _docRepository.GetDocById(result.SecondHolderId ?? 0);
            result.SecondHolderData = holder2.FirstOrDefault(); 
            var holder3 = await _docRepository.GetDocById(result.ThirdHolderId ?? 0);
            result.ThirdHolderData = holder3.FirstOrDefault();

            return result;
		}


        public async Task<IEnumerable<DocDto>> GetHolderByStockId(long id)
        {
            var stock = await _StockRepository.GetStockById(id);
            long?[] holdersids = { stock.FirstHolderId, stock.SecondHolderId, stock.ThirdHolderId };
            var holders = await _docRepository.GetDocByIds(holdersids);

            return holders;
        }

        public async Task<string> UploadStockExcel(UploadStockDto data1, int createdBy)
        {
            try
            {
                List<StockDto> dataList = new List<StockDto>();
                List<string> errorRows = new List<string>();

                if (createdBy != null && createdBy > 0)
                {
                    if (data1.DocFile != null && data1.DocFile.Length > 0)
                    {
                        using (var stream = data1.DocFile.OpenReadStream())
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = _ => new ExcelDataTableConfiguration()
                                    {
                                        UseHeaderRow = true
                                    }
                                });

                                var worksheet = result.Tables[0];

                                for (int row = 0; row < worksheet.Rows.Count; row++)
                                {
                                    var companyName = CleanTextOrNull(worksheet.Rows[row][0].ToString())?.ToUpper();
                                    var folioNo = CleanTextOrNull(worksheet.Rows[row][4].ToString())?.ToUpper();
                                    var companyId = 0;
                                    var holder1Name = CleanTextOrNull(worksheet.Rows[row][1].ToString())?.ToUpper();
                                    var holder2Name = CleanTextOrNull(worksheet.Rows[row][2].ToString())?.ToUpper();
                                    var holder3Name = CleanTextOrNull(worksheet.Rows[row][3].ToString())?.ToUpper();
                                    long? holder1Id = 0;
                                    long? holder2Id = 0;
                                    long? holder3Id = 0;

                                    if (companyName != null && folioNo != null)
                                    {
                                        var company = await _companyRepository.GetCompanyByName(companyName);
                                        if (company.Any())
                                        {
                                            companyId = company.FirstOrDefault().Id;
                                            var existingstock = await _StockRepository.GetStockByinfo(data1.CustomerId ?? 0, companyId, folioNo);
                                            if (existingstock.Any())
                                            {
                                                errorRows.Add($"row {row + 1}-Stock details Already Exists");
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            errorRows.Add($"row {row + 1}-Company Does not Exists");
                                            continue;
                                        }

                                        var holder1 = await _docRepository.GetDocByName(holder1Name);
                                        holder1Id = holder1.Any() ? holder1.FirstOrDefault().Id : 0;
                                        if (string.IsNullOrEmpty(holder1Name))
                                        {
                                            errorRows.Add($"row {row + 1}-First Holder Name cannot be Empty");
                                            continue;
                                        }

                                        var holder2 = await _docRepository.GetDocByName(holder2Name);
                                        holder2Id = holder2.Any() ? holder2.FirstOrDefault().Id : null;
                                        if (!string.IsNullOrEmpty(holder2Name) && holder2Id < 1)
                                        {
                                            errorRows.Add($"row {row + 1}-Second Holder Not Found in records");
                                            continue;
                                        }

                                        var holder3 = await _docRepository.GetDocByName(holder3Name);
                                        holder3Id = holder3.Any() ? holder3.FirstOrDefault().Id : null;
                                        if (!string.IsNullOrEmpty(holder3Name) && holder3Id < 1)
                                        {
                                            errorRows.Add($"row {row + 1}-Third Holder Not Found in records");
                                            continue;
                                        }

                                    }

                                    if (companyName == null)
                                    {
                                        errorRows.Add($"row {row + 1}-Company cannot be Empty");
                                        continue;
                                    }

                                    if (folioNo == null)
                                    {
                                        errorRows.Add($"row {row + 1}-FolioNo cannot be Empty");
                                        continue;
                                    }

                                    var claimstatus = CleanTextOrNull(worksheet.Rows[row][5].ToString())?.ToUpper();
                                    double actualQty = TryParseDouble(CleanTextOrNull(worksheet.Rows[row][6].ToString()));
                                    double QTY = TryParseDouble(CleanTextOrNull(worksheet.Rows[row][7].ToString()));
                                    double Rate = TryParseDouble(CleanTextOrNull(worksheet.Rows[row][8].ToString()));
                                    double brokerage = TryParseDouble(CleanTextOrNull(worksheet.Rows[row][9].ToString()));
                                    var PTBF = CleanTextOrNull(worksheet.Rows[row][10].ToString())?.ToUpper();
                                    var Remarks = CleanTextOrNull(worksheet.Rows[row][11].ToString())?.ToUpper();

                                    string format = "yyyy-MM-dd";

                                    try
                                    {
                                        var newItem = new StockDto
                                        {
                                            CustomerId = data1.CustomerId,
                                            FolioNo = folioNo,
                                            ClaimStatus = claimstatus,
                                            ActualQty = actualQty,
                                            Qty = QTY,
                                            Rate = Rate,
                                            Brokerage = brokerage,
                                            Ptbf = PTBF,
                                            Remarks = Remarks,
                                            CompanyId = companyId,
                                            FirstHolderId = holder1Id,
                                            SecondHolderId = holder2Id,
                                            ThirdHolderId = holder3Id,
                                            IsProcessed = false,
                                            CreatedAt = DateTime.Now,
                                            CreatedBy = createdBy,
                                            IsActive = true,
                                        };
                                        dataList.Add(newItem);
                                    }
                                    catch (Exception ex)
                                    {
                                        return $"Error in Row {row + 1}: {ex.Message}";
                                    }
                                }

                                // Insert the data
                                var data = await _StockRepository.AddStock(dataList);
                                if (data != null)
                                {
                                    if (!data.Any())
                                    {
                                        return $"{(errorRows.Any() ? $" Error while Processing:\n{string.Join("\n", errorRows)}" : "")}";
                                    }
                                    else
                                    {
                                        if (errorRows.Any())
                                        {

                                            return $"ExcelData processed successfully. Added {data.Count()} New entries." +
                                                    $"{(errorRows.Any() ? $" Error while Processing:\n [{string.Join("\n", errorRows)}]" : "")}";
                                        }

                                        return $"ExcelData processed Successfully. Added {data.Count()} New entries.";
                                    }
                                }
                                else
                                {
                                    return "Failed To process ExcelData";
                                }
                            }
                        }
                    }
                    else
                    {
                        return "No file uploaded.";
                    }
                }
                else
                {
                    return "Unable To Process.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        //Helper functions
        string CleanTextOrNull(string input)
        {
            var cleaned = input.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Trim();
            return string.IsNullOrEmpty(cleaned) ? null : cleaned;
        }

        private double TryParseDouble(string value)
        {
            double result;
            return double.TryParse(value, out result) ? result : 0;
        }

        public async Task<Int32> UpdateNomineeJson(long id, string jsonString, int updatedBy)
        {
            var isExist = await _StockRepository.GetStockById(id);
            var data = isExist;

            if (isExist.Id > 0)
            {
                data.NomineeJson = jsonString;
                data.UpdatedBy = updatedBy;
                data.UpdatedAt = DateTime.Now;

                List<StockDto> updateList = new List<StockDto> { data };
                var result = await _StockRepository.UpdateStock(updateList);
                if (result.Any())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -2;
            }

        }

        public async Task<dynamic> GetStocksStatusCount()
        {
            var allstocks = await _StockRepository.GetStock();

            var result = new
            {
                pending = allstocks.Where(x => x?.ClaimStatus.ToLower() == "pending").Count(),
                followup = allstocks.Where(x => x.ClaimStatus.ToLower() == "follow up").Count(),
                iepf = allstocks.Where(x => x.ClaimStatus.ToLower() == "iepf").Count(),
                iprs = allstocks.Where(x => x.ClaimStatus.ToLower() == "iepf post receipt submit").Count(),
                drf = allstocks.Where(x => x.ClaimStatus.ToLower() == "drf form submitted").Count(),
                demate = allstocks.Where(x => x.ClaimStatus.ToLower() == "demated").Count(),
                completed = allstocks.Where(x => x.ClaimStatus.ToLower() == "completed").Count(),
            };

            return result;
        }

        public async Task<dynamic> GetStocksByStatus()
        {
            var status = "iepf";
            var allstocks = await _StockRepository.GetStocksByStatus(status);

            var stockids = allstocks.Select(x => x.Id).ToArray();
            var tracing = await _trackingRepository.GetTrackingbyStockIds(stockids);

            var result = from allstock in allstocks
                         select new
                         {
                             allstock.Id,
                             allstock.CompanyName,
                             allstock.FirstHolderName,
                             allstock.ClaimStatus,
                             allstock.FolioNo,
                             allstock.Qty,
                             trackDate = tracing.Where(x => x.StockId == allstock.Id ).OrderByDescending(x => x.Id).Take(1).Select(x=>x.DateofSubmission)
                        };

            return result;
        }

        public async Task<dynamic> GetStocksByStatusName(string status)
        {
            var allstocks = await _StockRepository.GetStocksByStatus(status);

            var stockids = allstocks.Select(x => x.Id).ToArray();
            var tracing = await _trackingRepository.GetTrackingbyStockIds(stockids);

            var result = from allstock in allstocks
                         select new
                         {
                             allstock.Id,
                             allstock.CompanyName,
                             allstock.FirstHolderName,
                             allstock.ClaimStatus,
                             allstock.FolioNo,
                             allstock.Qty,
                             trackDate = tracing.Where(x => x.StockId == allstock.Id).OrderByDescending(x => x.Id).Take(1).Select(x => x.DateofSubmission)
                         };

            return result;
        }
    }
}
