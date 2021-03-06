﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Data.Infrastructure
{
	public abstract class DServiceBase<T> : DataService<T>
	{
        /// <summary>
        /// Initializes the service.
        /// </summary>
        /// <param name="config">The config.</param>
		public static void InitializeService(DataServiceConfiguration config)
		{
			config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
			config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
			config.DataServiceBehavior.AcceptCountRequests = true;
			config.DataServiceBehavior.AcceptProjectionRequests = true;
			config.UseVerboseErrors = true;
		}

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="args">The args.</param>
		protected override void HandleException(HandleExceptionArgs args)
		{
			string errorMessage = args.Exception.Message;
			var exception = args.Exception;

			if (exception.InnerException != null)
			{
				exception = exception.InnerException;
				errorMessage += " " + exception.Message;
			}

			var dbEntityValidationException = exception as DbEntityValidationException;

			if (dbEntityValidationException != null)
			{
				errorMessage = errorMessage + " " + String.Join(" ", dbEntityValidationException.EntityValidationErrors.First().ValidationErrors);
			}
			
			args.Exception = new DataServiceException(500, errorMessage);
		}
	}
}
