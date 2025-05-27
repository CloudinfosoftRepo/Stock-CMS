using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Stock_CMS.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMenuRepository _menuRepository;

        public PermissionService(IPermissionRepository permissionRepository, IMenuRepository menuRepository)
        {
            _permissionRepository = permissionRepository;
            _menuRepository = menuRepository;
        }

        public async Task<IEnumerable<MenuDto>> GetMenu()
        {
            try
            {
                var menu = await _menuRepository.GetMenu();

                var result = from m in menu
                             select new MenuDto
                             {
                                 Id = m.Id,
                                 Name = m.Name,
                                 Url = m.Url,
                                 ParentId = m.ParentId,
                                 Actions = m.Actions,
                                 IsActive = m.IsActive,
                                 CreatedAt = m.CreatedAt,
                                 CreatedBy = m.CreatedBy,
                                 UpdatedAt = m.UpdatedAt,
                                 UpdatedBy = m.UpdatedBy,
                                 ActionList = m.Actions.Split(",").Select(x => new ActionItem { Action = x.Trim(), IsEnabled = true }).ToList(),
                             };
                return result;
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
        public async Task<IEnumerable<PermissionDto>> GetPermissionsByUserMenu(int userId, int menuId)
        {
            var userperms = await _permissionRepository.GetPermissionsByUserMenu(userId, menuId);

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
            return userperms;
        }

        public async Task<dynamic> GetMenuByUser(int UserId)
        {
            try
            {
                var menu = await _menuRepository.GetMenu();
                var userperms = await _permissionRepository.GetPermissionsByUser(UserId);


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
                                                               Actions = m.Actions,
                                                               IsActive = m.IsActive,
                                                               CreatedAt = m.CreatedAt,
                                                               CreatedBy = m.CreatedBy,
                                                               UpdatedAt = m.UpdatedAt,
                                                               UpdatedBy = m.UpdatedBy,
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
                    .Select(g => new
                    {
                        ParentId = g.Key,
                        ParentName = fullmenu.FirstOrDefault(y => y.Id == g.Key)?.Name,
                        Sequence = fullmenu.FirstOrDefault(y => y.Id == g.Key)?.Sequence,
                        Items = g.ToList()
                    })
                    .ToList().OrderBy(x => x.Sequence);

                return grouped;
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
        public async Task<dynamic> GetMenuByRole(int RoleId)
        {
            try
            {
                var menu = await _menuRepository.GetMenu();
                var userperms = await _permissionRepository.GetPermissionsByRole(RoleId);


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
                                                               Actions = m.Actions,
                                                               IsActive = m.IsActive,
                                                               CreatedAt = m.CreatedAt,
                                                               CreatedBy = m.CreatedBy,
                                                               UpdatedAt = m.UpdatedAt,
                                                               UpdatedBy = m.UpdatedBy,
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
                    .Select(g => new
                    {
                        ParentId = g.Key,
                        ParentName = fullmenu.FirstOrDefault(y => y.Id == g.Key)?.Name,
                        Sequence = fullmenu.FirstOrDefault(y => y.Id == g.Key)?.Sequence,
                        Items = g.ToList()
                    })
                    .ToList();

                return grouped;
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }

        public async Task<IEnumerable<PermissionDto>> GetPermissionsByRole(int roleId)
        {
            try
            {
                var permissions = await _permissionRepository.GetPermissionsByRole(roleId);

                var result = from p in permissions
                             select new PermissionDto
                             {
                                 Id = p.Id,
                                 MenuId = p.MenuId,
                                 RoleId = p.RoleId,
                                 UserId = p.UserId,
                                 Actions = p.Actions,
                                 IsActive = p.IsActive,
                                 CreatedAt = p.CreatedAt,
                                 CreatedBy = p.CreatedBy,
                                 UpdatedAt = p.UpdatedAt,
                                 UpdatedBy = p.UpdatedBy,
                             };
                return result;
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
        public async Task<IEnumerable<PermissionDto>> GetPermissionsByUserId(int userId)
        {
            try
            {
                var permissions = await _permissionRepository.GetPermissionsByUser(userId);

                var result = from p in permissions
                             select new PermissionDto
                             {
                                 Id = p.Id,
                                 MenuId = p.MenuId,
                                 RoleId = p.RoleId,
                                 UserId = p.UserId,
                                 Actions = p.Actions,
                                 IsActive = p.IsActive,
                                 CreatedAt = p.CreatedAt,
                                 CreatedBy = p.CreatedBy,
                                 UpdatedAt = p.UpdatedAt,
                                 UpdatedBy = p.UpdatedBy,
                                 ActionList = p.Actions != null ? JsonConvert.DeserializeObject<List<ActionItem>>(p.Actions) : null
                             };
                return result;
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }

        public async Task<dynamic> UpdatePermissionByUserMenu(PermissionDto perm)
        {
            try
            {
                List<PermissionDto> list = new List<PermissionDto>();

                var existingperm = await _permissionRepository.GetPermissionsByUserMenu(perm.UserId ?? 0, perm.MenuId ?? 0);
                var existing = existingperm.FirstOrDefault();

                if (existingperm != null && existingperm.Any())
                {
                    try
                    {
                        var actions = JsonConvert.DeserializeObject<IEnumerable<ActionItem>>(perm.Actions);
                    }
                    catch (Exception ex)
                    {
                        perm.Actions = null;
                    }


                    PermissionDto item = new()
                    {
                        Id = existing.Id,
                        RoleId = existing.RoleId,
                        UserId = perm.UserId != 0 ? perm.UserId : existing.UserId,
                        MenuId = perm.MenuId != 0 ? perm.MenuId : existing.MenuId,
                        Actions = perm.Actions ?? existing.Actions,
                        IsActive = perm.IsActive ?? existing.IsActive,
                        CreatedAt = existing.CreatedAt,
                        CreatedBy = existing.CreatedBy,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = perm.UpdatedBy,
                    };

                    list.Add(item);
                    var result = await _permissionRepository.UpdatePermission(list);
                    if (result != null)
                    {
                        return new { Status = true, Message = "Permission Updated Successfully" };
                    }
                    return new { Status = false, Message = "Failed To Update Permission" };
                }
                return new { Status = false, Message = "Permissions Are not configured for this User, please update User" };

            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
        public async Task<dynamic> UpdatePermissionByRoleMenu(PermissionDto perm)
        {
            try
            {
                var existingpermList = await _permissionRepository.GetPermissionsByRoleMenu(perm.RoleId ?? 0, perm.MenuId ?? 0);

                if (existingpermList != null && existingpermList.Any())
                {
                    try
                    {
                        var actions = JsonConvert.DeserializeObject<IEnumerable<ActionItem>>(perm.Actions);
                    }
                    catch (Exception ex)
                    {
                        perm.Actions = null;
                    }

                    existingpermList.ToList().ForEach(x => { x.Actions = perm.Actions ?? x.Actions; x.UpdatedAt = DateTime.Now; x.UpdatedBy = perm.UpdatedBy; });

                    var result = await _permissionRepository.UpdatePermission(existingpermList);
                    if (result != null)
                    {
                        return new { Status = true, Message = "Permission Updated Successfully" };
                    }
                    return new { Status = false, Message = "Failed To Update Permission" };
                }
                return new { Status = false, Message = "Permissions Are not configured for this Role, please update Users" };

            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
    }
}
