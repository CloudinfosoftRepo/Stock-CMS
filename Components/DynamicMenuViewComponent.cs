using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;


public class DynamicMenuViewComponent : ViewComponent
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IMenuRepository _menuRepository;
    public DynamicMenuViewComponent(IPermissionRepository permissionRepository, IMenuRepository menuRepository)
    {
        _permissionRepository = permissionRepository;
        _menuRepository = menuRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        try
        {
            var userId = int.Parse(Request.Cookies["UserId"].ToString());

            try
            {
                var menu = await _menuRepository.GetMenu();
                var userperms = await _permissionRepository.GetPermissionsByUser(userId);


                try
                {
                    userperms.ToList().ForEach(x =>
                    {
                        x.ActionList = x.Actions != null ? JsonConvert.DeserializeObject<List<ActionItem>>(x.Actions) : null;
                    });
                }
                catch (Exception ex)
                {
                    userperms.ToList().ForEach(x =>
                    {
                        x.ActionList = null;
                    });
                }

                var result = menu.GroupJoin(userperms, m => m.Id,
                                                           up => up.MenuId,
                                                           (m, upGroup) => new MenuDto
                                                           {
                                                               Id = m.Id,
                                                               Name = m.Name,
                                                               Url = m.Url,
                                                               ParentId = m.ParentId,
                                                               Icon = m.Icon,
                                                               Actions = m.Actions,
                                                               IsActive = m.IsActive,
                                                               CreatedAt = m.CreatedAt,
                                                               UpdatedAt = m.UpdatedAt,
                                                               ActionList = !string.IsNullOrEmpty(m.Actions)
                                                                                ? m.Actions.Split(',')
                                                                                    .Select(action =>
                                                                                    {
                                                                                        string trimmedAction = action.Trim();

                                                                                        bool isEnabledForAllUsers = upGroup.Any()
                                                                                                                    && upGroup.All(u => u.ActionList != null && u.ActionList.Any(x => x.Action.ToUpper() == trimmedAction.ToUpper() && x.IsEnabled));

                                                                                        return new ActionItem
                                                                                        {
                                                                                            Action = trimmedAction,
                                                                                            IsEnabled = isEnabledForAllUsers
                                                                                        };
                                                                                    })
                                                                                    .ToList()
                                                                                : new List<ActionItem>()
                                                           }).ToList();


                var fullmenu = await _menuRepository.GetMenuWithParents();
                var grouped = result
                    .GroupBy(x => x.ParentId)
                    .Select(g => new MenuViewModel()
                    {
                        ParentId = g.Key,
                        Icon = fullmenu.Where(x => x.Id == g.Key).Select(x => x.Icon).FirstOrDefault(),
                        ParentMenuName = fullmenu.FirstOrDefault(y => y.Id == g.Key)?.Name,
                        Menus = g.Where(x => x.ActionList.Any(y => y.Action.ToUpper() == "VIEW" && y.IsEnabled)).ToList()
                    }).Where(x => x.Menus != null && x.Menus.Any())
                    .ToList();

                return View(grouped);
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }








            //     var menus = (from menu in _context.TblMenus
            //                  join permission in _context.TblPermissions
            //                  on menu.Id equals permission.MenuId 
            //                  //into menuPermissions
            //                  //from permission in menuPermissions.DefaultIfEmpty()
            //                  where (permission == null ||
            //                        (permission.IsActive &&
            //                         (permission.UserId == userId || permission.RoleId == userRoleId)))
            //                         && permission.Code == "view"
            //orderby menu.ParentId, menu.Order
            //                  select new Menudto
            //                  {
            //                      Id = menu.Id,
            //                      Name = menu.Name,
            //                      Url = menu.Url,
            //                      Icon = menu.Icon,
            //                      ParentId = menu.ParentId,
            //                      ParentMenuName = menu.ParentId != null
            //                         ? _context.TblMenus.Where(m => m.Id == menu.ParentId).FirstOrDefault().Name
            //                         : null
            //                  }).ToList();


            //     // Group the menus by ParentId
            //     var groupedMenus = menus.DistinctBy(m => m.Id)
            //         .GroupBy(m => m.ParentId)
            //         .Select(g => new MenuViewModel
            //         {
            //             ParentId = g.Key,
            //             Icon = menus.Where(x => x.Id == g.Key).Select(x => x.Icon).FirstOrDefault(),
            //             ParentMenuName = g.FirstOrDefault()?.ParentMenuName,
            //             Menus = g.ToList()
            //         }).Where(x => x.ParentId != null)
            //         .ToList();

            //     return View(groupedMenus);
        }
        catch (Exception ex)
        {
            return View(ex);
        }
    }

}
