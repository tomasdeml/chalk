using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using PowerArgs;

namespace Chalk.Actions
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Parameters
    {
        [ArgShortcut("a")]
        [ArgRequired]
        public ActionKind Action { get; set; }

        [ArgShortcut("wp")]
        [ArgDefaultValue(@".\")]
        public string LocalWorkspacePath { get; set; }

        [ArgShortcut("vcp")]
        [DefaultValue("Vault.exe")]
        public string VaultCommandLineClientPath { get; set; }

        [ArgShortcut("vst")]
        [ArgDefaultValue(0)]
        [ArgRange(0, Int32.MaxValue)]
        public int VaultServerTimeOutInSeconds { get; set; }

        [ArgShortcut("vh")]
        public string VaultHost { get; set; }

        [ArgShortcut("vu")]
        public string VaultUserName { get; set; }

        [ArgShortcut("vp")]
        public string VaultPassword { get; set; }

        [ArgShortcut("vrn")]
        public string VaultRepositoryName { get; set; }

        [ArgShortcut("vrp")]
        public string VaultRepositoryPath { get; set; }

        [ArgShortcut("aed")]
        public string CommitAuthorEmailDomain { get; set; }

        [ArgShortcut("ddd")]
        public bool DisableRepositoryDeletionDetection { get; set; }

        public void Validate()
        {
            var validationResult = GetValidator().Validate(this);
            if (validationResult.IsValid)
                return;

            string message = CreateMessageForValidationErrors(validationResult.Errors); 
            throw new ValidationException(message, validationResult.Errors);
        }

        static string CreateMessageForValidationErrors(IEnumerable<ValidationFailure> errors)
        {
            IEnumerable<string> messages = errors.Select(x => x.ErrorMessage);
            return "Validation of one or more parameters failed: " + string.Join(",", messages);
        }

        IValidator<Parameters> GetValidator()
        {
            return Action == ActionKind.WorkspaceInit
                ? (IValidator<Parameters>) new WorkspaceInitActionValidator()
                : new VaultExportActionValidator();
        }

        abstract class RepositoryRelatedActionValidator : AbstractValidator<Parameters>
        {
            protected RepositoryRelatedActionValidator()
            {
                RuleFor(p => p.LocalWorkspacePath).NotEmpty();
                RuleFor(p => p.CommitAuthorEmailDomain).NotEmpty();
            }
        }

        class WorkspaceInitActionValidator : RepositoryRelatedActionValidator { }

        class VaultExportActionValidator : RepositoryRelatedActionValidator
        {
            public VaultExportActionValidator()
            {
                RuleFor(p => p.VaultCommandLineClientPath).NotEmpty();
                RuleFor(p => p.VaultHost).NotEmpty();
                RuleFor(p => p.VaultUserName).NotEmpty();
                RuleFor(p => p.VaultPassword).NotEmpty();
                RuleFor(p => p.VaultRepositoryName).NotEmpty();
                RuleFor(p => p.VaultRepositoryPath).NotEmpty();
            }
        }
    }
}
