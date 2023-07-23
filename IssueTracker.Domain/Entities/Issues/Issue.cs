﻿using IssueTracker.Domain.Constants;
using IssueTracker.Domain.Entities.IssueImage;
using IssueTracker.Domain.Models.Issue;
using System.Text.Json.Serialization;

namespace IssueTracker.Domain.Entities.Issues
{
    public class Issue : IIssue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AssignedTo { get; set; }
        public int CreatedBy { get; set; }
        public int? ClosedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public List<IFile> Files { get; set; }
        public DateTime? DeadLine { get; set; }
        public IssueStatus Status { get; set; } = IssueStatus.BackLog;

        public Issue()
        {
            Files = new List<IFile>();
        }
        public Issue(IssueModel issueModel)
        {
            Id = issueModel.Id;
            Name = issueModel.Name;
            Description = issueModel.Description;
            AssignedTo = issueModel.AssignedTo;
            CreatedBy = issueModel.CreatedBy;
            ClosedBy = issueModel.ClosedBy;
            ClosedOn = issueModel.ClosedOn;
            DeadLine = issueModel.DeadLine;
            Files = new List<IFile>();

            //foreach (var imageModel in issueModel.Images)
            //{
            //    var image = new Image();
            //    image.ImageGuid = imageModel.ImageGuid;
            //    image.ImagePath = imageModel.ImagePath;
            //    image.Bas64Image = imageModel.Base64Image;
            //    Images.Add(image);
            //}
        }
    }
    public class File : IFile
    {
        public string ImagePath { get; set; }
        public Guid? ImageGuid { get; set; }
        public File()
        {

        }
        public File(ImagesModel imagesModel)
        {
            //Bas64Image = imagesModel.Base64Image;
        }
    }
}
