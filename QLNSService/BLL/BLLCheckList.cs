using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLNSService.Data;
using QLNSService.Enum;
using QLNSService.Models;
using System.Web.Mvc;

namespace QLNSService.BLL
{
    public static class BLLCheckList
    {
        public static CheckListModel Get_Collection()
        {
            try
            {
                var db = new ProManaEntities();
                var model = new CheckListModel();
                model.Head.AddRange(db.P_JobGroup.Where(x => !x.IsDeleted && x.IsShow).Select(x => new JobGroupModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    OrderIndex = x.OrderIndex,
                    IsShow = x.IsShow
                }).OrderBy(x => x.OrderIndex).ToList());
                if (model.Head.Count > 0)
                {
                    var jGroups = db.P_PM_JobGroup.Where(x => !x.IsDeleted && x.P_JobGroup.IsShow).Select(x => new ModelTongHop()
                    {
                        Id = x.Id,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        JobGroupId = x.JobGroupId,
                        JobGroupName = x.P_JobGroup.Name,
                        Node = x.Node,
                        Note = x.Note,
                        OrderIndex = x.OrderIndex,
                        ParentId = x.ParentId,
                        PartOfOrganId = x.PartOfOrganId,
                        IsHasRHomebus = x.IsHasRHomebus,
                        CompanyId = x.CompanyId,
                        PercentComplete = x.PercentComplete,
                        StatusId = x.StatusId,
                        RHomebusTitle = x.RHomebusTitle
                    }).ToList();
                    if (jGroups.Count > 0)
                    {
                        var nodes = jGroups.Select(x => x.Node).Distinct().ToList();
                        for (int i = 0; i < nodes.Count(); i++)
                        {
                            var ids = nodes[i].Substring(0, nodes[i].Length - 1).Split(',').Select(x => Convert.ToInt32(x)).ToList();
                            var pros = db.P_ProManagement.Where(x => !x.IsDeleted && ids.Contains(x.Id)).ToList();
                            if (pros.Count > 0)
                            {
                                var groups = jGroups.Where(x => x.Node == nodes[i]);
                                if (groups != null && groups.Count() > 0)
                                {
                                    foreach (var item in groups)
                                    {
                                        item.OrderName = pros.FirstOrDefault(x => x.Id == ids[1]).Name;
                                        item.CustomerName = pros.FirstOrDefault(x => x.Id == ids[2]).Name;
                                    }
                                }
                                var bditem = new BodyItem();
                                bditem.Items.AddRange(groups);
                                model.Body.Add(bditem);
                            }

                        }
                    }
                }
                return model;
            }
            catch (Exception)
            { }
            return null;
        }

        public static CheckListModel Get_JGroupId(int jGroupId, int userId)
        {
            try
            {
                var db = new ProManaEntities();
                var model = new CheckListModel();
                model.Title = db.P_JobGroup.FirstOrDefault(x => !x.IsDeleted && x.Id == jGroupId).Name;
                model.Head.AddRange(db.P_Job.Where(x => !x.IsDeleted && x.IsShow).Select(x => new JobGroupModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    OrderIndex = x.OrderIndex,
                    IsShow = x.IsShow
                }).OrderBy(x => x.OrderIndex).ToList());
                if (model.Head.Count > 0)
                {
                    var jGroups = db.P_PM_Job.Where(x => !x.IsDeleted && !x.P_PM_JobGroup.IsDeleted && x.EmployeeId == userId && x.P_Job.IsShow && x.P_PM_JobGroup.JobGroupId == jGroupId).Select(x => new ModelTongHop()
                   {
                       Id = x.Id,
                       StartDate = x.TimeStart,
                       EndDate = x.TimeEnd,
                       JobId = x.JobId,
                       JobName = x.P_Job.Name,
                       JobGroupName = x.P_PM_JobGroup.P_JobGroup.Name,
                       Node = x.P_PM_JobGroup.Node,
                       OrderIndex = x.OrderIndex,
                       CompanyId = x.CompanyId,
                       PercentComplete = x.PercentComplete,
                       StatusId = x.StatusId,
                   }).ToList();
                    if (jGroups.Count > 0)
                    {
                        var nodes = jGroups.Select(x => x.Node).Distinct().ToList();
                        for (int i = 0; i < nodes.Count(); i++)
                        {
                            var ids = nodes[i].Substring(0, nodes[i].Length - 1).Split(',').Select(x => Convert.ToInt32(x)).ToList();
                            var pros = db.P_ProManagement.Where(x => !x.IsDeleted && ids.Contains(x.Id)).ToList();
                            if (pros.Count > 0)
                            {
                                var groups = jGroups.Where(x => x.Node == nodes[i]);
                                if (groups != null && groups.Count() > 0)
                                {
                                    foreach (var item in groups)
                                    {
                                        var obj = pros.FirstOrDefault(x => x.Id == ids[1]);
                                        item.OrderName = obj != null ? obj.Name : string.Empty;
                                        var obj1 = pros.FirstOrDefault(x => x.Id == ids[2]);
                                        item.CustomerName = obj1 != null ? obj1.Name : string.Empty;
                                    }
                                }
                                var bditem = new BodyItem();
                                bditem.Items.AddRange(groups);
                                model.Body.Add(bditem);
                            }

                        }
                    }
                }
                return model;
            }
            catch (Exception)
            { }
            return null;
        }

        public static List<SelectListItem> GetJobGroupSelect()
        {
            try
            {
                var db = new ProManaEntities();
                var jobGroups = db.P_JobGroup.Where(x => !x.IsDeleted && x.IsShow).OrderBy(x => x.OrderIndex).ToList(); ;
                var listSelect = new List<SelectListItem>();
                listSelect.Add(new SelectListItem() { Value = "0", Text = "Không có dữ liệu" });
                if (jobGroups != null && jobGroups.Count() > 0)
                {
                    listSelect.RemoveAt(0);
                    listSelect.Add(new SelectListItem() { Value = "0", Text = "... Chọn Nhóm Công Việc ..." });
                    listSelect.AddRange(jobGroups.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }).ToList());
                }
                return listSelect;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}