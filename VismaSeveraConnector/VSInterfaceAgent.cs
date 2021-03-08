/*

This code is provided as a reference sample only and has no explicit or implicit support
as to its nature, completeness, nor function.  Please see the license file
(included in this repository) for more details.

*/

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Severa.Entities.API;
using System.Globalization;

namespace VismaSeveraConnector
{
	/// <summary>
	/// A class that handles the operations to Severa web service interface.
	/// </summary>
	class VSInterfaceAgent
	{
		/// <summary>
		/// A common minimum range for the methods that do support
		/// modification data, but for which we can't use partiality 
		/// i.e. because of indirectity.
		/// </summary>
		private DateTime COMMON_MIN_RANGE = new DateTime( 1998, 04, 30 );
		private string m_apiKey;

		private BasicHttpBinding m_binding;
		private EndpointAddress m_endpoint;


		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="ApiKey">Severa API key.</param>
		/// <param name="Endpoint">Web service endpoint.</param>
		public VSInterfaceAgent( string ApiKey, string Endpoint ) : this( ApiKey, Endpoint, "" ) { }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="ApiKey">Severa API key.</param>
		/// <param name="Endpoint">Web service endpoint.</param>
		/// <param name="certFile">Certificate file location.</param>
		public VSInterfaceAgent( string ApiKey, string Endpoint, string certFile )
		{
			m_apiKey = ApiKey;

			// Create binding and endpoint.
			m_binding = new BasicHttpBinding();
			m_binding.Security.Mode = BasicHttpSecurityMode.Transport;
			m_binding.MaxReceivedMessageSize = 67108864;
			m_binding.MaxBufferSize = 67108864;
			m_binding.TransferMode = TransferMode.Streamed;
			m_endpoint = new EndpointAddress( Endpoint );

			// If certificate was defined, set it.
			if( certFile.ToLower() == "acceptany" )
			{
				Certifier.SetUnsafePolicy();
			}
		}

		#region Get single items by GUID.

		/// <summary>
		/// Returns a language by GUID.
		/// </summary>
		/// <param name="GUID">Account GUID.</param>
		/// <returns>The Language object if found.</returns>
		internal Language GetOneLanguage( string GUID )
		{
			// Create a channel factory.
			using( ChannelFactory<ILanguage> channelFactory = new ChannelFactory<ILanguage>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in
					channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>()
						as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and try to retrieve the account.
				ILanguage chan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )chan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return chan.GetLanguageByGUID( GUID );
				}
			}
		}

		/// <summary>
		/// Returns a pricelist by GUID.
		/// </summary>
		/// <param name="GUID">Account GUID.</param>
		/// <returns>The Pricelist object if found.</returns>
		internal Pricelist GetOnePricelist( string GUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IPricelist> channelFactory = new ChannelFactory<IPricelist>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in
					channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>()
						as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and try to retrieve the account.
				IPricelist chan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )chan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return chan.GetPricelistByGUID( GUID );
				}
			}
		}

		/// <summary>
		/// Returns a timezone by GUID.
		/// </summary>
		/// <param name="GUID">Account GUID.</param>
		/// <returns>The Timezone object if found.</returns>
		internal Timezone GetOneTimezone( string GUID )
		{
			// Create a channel factory.
			using( ChannelFactory<ITimezone> channelFactory = new ChannelFactory<ITimezone>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in
					channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>()
						as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and try to retrieve the account.
				ITimezone chan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )chan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return chan.GetTimezoneByGUID( GUID );
				}
			}
		}

		/// <summary>
		/// Returns an account by GUID.
		/// </summary>
		/// <param name="GUID">LeadSource GUID.</param>
		/// <returns>The LeadSource object if found.</returns>
		internal LeadSource GetOneLeadSource( string GUID )
		{
			// Create a channel factory.
			using( ChannelFactory<ILeadSource> channelFactory = new ChannelFactory<ILeadSource>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in
					channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>()
						as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and try to retrieve the account.
				ILeadSource chan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )chan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return chan.GetLeadSourceByGUID( GUID );
				}
			}
		}

		/// <summary>
		/// Returns an account by GUID.
		/// </summary>
		/// <param name="AccountGUID">Account GUID.</param>
		/// <returns>The account object if found.</returns>
		public Account GetOneAccount( string AccountGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IAccount> channelFactory = new ChannelFactory<IAccount>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in
					channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>()
						as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and try to retrieve the account.
				IAccount accountChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )accountChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return accountChan.GetAccountByGUID( AccountGUID );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="GUID"></param>
		/// <returns></returns>
		internal Invoice GetOneInvoice( string GUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IInvoice> channelFactory = new ChannelFactory<IInvoice>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>()
							as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and retrieve company by GUID.
				IInvoice invoiceChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )invoiceChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return invoiceChan.GetInvoiceByGUID( GUID );
				}
			}
		}

		/// <summary>
		/// Returns an industry bu GUID.
		/// </summary>
		/// <param name="IndustryGUID">Industry GUID.</param>
		/// <returns>The industry object if found.</returns>
		public Industry GetOneIndustry( string IndustryGUID )
		{
			Industry returnIndustry = new Industry();
			Industry[] industriesArray;

			// Create a channel factory.
			using( ChannelFactory<IIndustry> channelFactory = new ChannelFactory<IIndustry>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and try to retrieve the account.
				// Unlike some other objects, we can't search industry by GUID.
				// We need to retrieve all objects and find the right one.
				IIndustry industryChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )industryChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );

					industriesArray = industryChan.GetIndustries();

					foreach( Industry _industry in industriesArray )
					{
						if( _industry.GUID == IndustryGUID )
						{
							returnIndustry = _industry;
							break;
						}
					}
				}
			}
			return returnIndustry;
		}

		/// <summary>
		/// Returns a tag by GUID.
		/// </summary>
		/// <param name="TagGUID">Tag GUID.</param>
		/// <returns>The tag object if found.</returns>
		public Tag GetOneTag( string TagGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<ITag> channelFactory = new ChannelFactory<ITag>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create the channel and retrieve the tag by GUID.
				ITag tagChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )tagChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return tagChan.GetTagByGUID( TagGUID );
				}
			}
		}


		/// <summary>
		/// Returns a product by GUID.
		/// </summary>
		/// <param name="ProductGUID">Product GUID</param>
		/// <returns>The product object if found.</returns>
		public Product GetOneProduct( string ProductGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IProduct> channelFactory = new ChannelFactory<IProduct>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and retrieve the product by GUID.
				IProduct productChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )productChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return productChan.GetProductByGUID( ProductGUID );
				}
			}
		}

		/// <summary>
		/// Returns a sales process by GUID.
		/// </summary>
		/// <param name="SalesProcessGUID">Sales process GUID.</param>
		/// <returns>The sales process object if found.</returns>
		public SalesProcess GetOneSalesProcess( string SalesProcessGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<ISalesProcess> channelFactory = new ChannelFactory<ISalesProcess>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and retrieve sales process by GUID.
				ISalesProcess salesProcessChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )salesProcessChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return salesProcessChan.GetSalesProcessByGUID( SalesProcessGUID );
				}
			}
		}

		/// <summary>
		/// Returns a product by GUID.
		/// </summary>
		/// <param name="ProductGUID">Product GUID</param>
		/// <returns></returns>
		public SalesStatus GetOneSalesStatus( string SalesStatusGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<ISalesStatus> channelFactory = new ChannelFactory<ISalesStatus>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and retrieve sales status by GUID.
				ISalesStatus salesStatusChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )salesStatusChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return salesStatusChan.GetSalesStatusByGUID( SalesStatusGUID );
				}
			}
		}

		/// <summary>
		/// Returns a copmany by GUID.
		/// </summary>
		/// <param name="CompanyGUID">Company GUID.</param>
		/// <returns>Company object if found.</returns>
		public Company GetOneCompany( string CompanyGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<ICompany> channelFactory = new ChannelFactory<ICompany>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and retrieve company by GUID.
				ICompany companyChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )companyChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return companyChan.GetCompanyByGUID( CompanyGUID );
				}
			}
		}

		/// <summary>
		/// Returns a contact by GUID.
		/// </summary>
		/// <param name="ContactGUID">Contact GUID.</param>
		/// <returns>Contact object if found.</returns>
		public Contact GetOneContact( string ContactGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IContact> channelFactory = new ChannelFactory<IContact>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and retrieve contact by GUID.
				IContact contactChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )contactChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return contactChan.GetContactByGUID( ContactGUID );
				}
			}
		}

		/// <summary>
		/// Returns a case by GUID.
		/// </summary>
		/// <param name="CaseGUID">Case GUID.</param>
		/// <returns>Case object if found.</returns>
		public Case GetOneCase( string CaseGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<ICase> channelFactory = new ChannelFactory<ICase>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 2147483647;
					}
				}
				// Create a channel and retrieve case by GUID.
				ICase caseChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )caseChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return caseChan.GetCaseByGUID( CaseGUID );
				}
			}
		}

		/// <summary>
		/// Returns an address by GUID.
		/// </summary>
		/// <param name="ContactGUID">Address GUID.</param>
		/// <returns>Address object if found.</returns>
		public Address GetOneAddress( string AddressGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IAddress> channelFactory = new ChannelFactory<IAddress>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and retrieve contact by GUID.
				IAddress addressChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )addressChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return addressChan.GetAddressByGUID( AddressGUID );
				}
			}
		}

		/// <summary>
		/// Returns a user by GUID.
		/// </summary>
		/// <param name="UserGUID">User GUID.</param>
		/// <returns>User object if found.</returns>
		public User GetOneUser( string UserGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IUser> channelFactory = new ChannelFactory<IUser>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and retrieve contact by GUID.
				IUser userChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )userChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return userChan.GetUserByGUID( UserGUID );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ContactGUID"></param>
		/// <returns></returns>
		public WorkType GetOneWorkType( string WorkTypeGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IWorkType> channelFactory = new ChannelFactory<IWorkType>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IWorkType worktypeChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )worktypeChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return worktypeChan.GetWorkTypeByGUID( WorkTypeGUID );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ContactGUID"></param>
		/// <returns></returns>
		public BusinessUnit GetOneBusinessUnit( string BusinessUnitGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IBusinessUnit> channelFactory = new ChannelFactory<IBusinessUnit>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IBusinessUnit businessunitChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )businessunitChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return businessunitChan.GetBusinessUnitByGUID( BusinessUnitGUID );
				}
			}
		}

		/// <summary>
		/// Returns a product category by GUID.
		/// </summary>
		/// <param name="ProductGUID">Product GUID</param>
		/// <returns></returns>
		public ProductCategory GetOneProductCategory( string PcGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IProductCategory> channelFactory = new ChannelFactory<IProductCategory>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IProductCategory pcChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )pcChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return pcChan.GetProductCategoryByGUID( PcGUID );
				}
			}
		}

		/// <summary>
		/// Returns an account by GUID.
		/// </summary>
		/// <param name="AccountGUID">Account GUID</param>
		/// <returns></returns>
		public Phase GetOnePhase( string PhaseGUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IPhase> channelFactory = new ChannelFactory<IPhase>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IPhase phaseChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )phaseChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return phaseChan.GetPhaseByGUID( PhaseGUID );
				}
			}
		}

		#endregion

		#region Get items modified since a certain timestamp.

		internal User[] GetCaseMembers( string GUID )
		{
			// Create a channel factory.
			using( ChannelFactory<ICase> channelFactory = new ChannelFactory<ICase>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as
							System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ICase chan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )chan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return chan.GetCaseMemberUsersByCaseGUID( GUID );
				}
			}
		}

		internal Pricelist[] GetAllPricelists()
		{
			// Create a channel factory.
			using( ChannelFactory<IPricelist> channelFactory = new ChannelFactory<IPricelist>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as
							System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IPricelist chan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )chan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return chan.GetPricelists();
				}
			}
		}

		internal User[] GetPhaseMembers( string GUID )
		{
			// Create a channel factory.
			using( ChannelFactory<IPhaseMember> channelFactory = new ChannelFactory<IPhaseMember>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as
							System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IPhaseMember chan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )chan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					string[] userGUIDs = chan.GetPhaseMembersByPhaseGUID( GUID, true );
					User[] users = new User[ userGUIDs.Count() ];
					for( int i = 0; i < userGUIDs.Count(); ++i )
					{
						users[ i ] = GetOneUser( userGUIDs[ i ] );
					}
					return users;
				}
			}
		}

		internal Timezone[] GetAllTimezones()
		{
			// Create a channel factory.
			using( ChannelFactory<ITimezone> channelFactory = new ChannelFactory<ITimezone>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as
							System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ITimezone chan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )chan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return chan.GetAllTimezones();
				}
			}
		}

		internal IEnumerable<Language> GetAllLanguages()
		{
			// Create a channel factory.
			using( ChannelFactory<ILanguage> channelFactory = new ChannelFactory<ILanguage>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as
							System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ILanguage chan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )chan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );

					// Find all languages in system
					List<Language> foundLanguages = new List<Language>();
					foreach( CultureInfo ci in CultureInfo.GetCultures( CultureTypes.InstalledWin32Cultures ) )
						try
						{
							if( chan.GetLanguageByIetfTag( ci.IetfLanguageTag ) != null )
								foundLanguages.Add( chan.GetLanguageByIetfTag( ci.IetfLanguageTag ) );
						}
						catch( System.ServiceModel.FaultException<Severa.API.NotFoundException> )
						{
							continue;
						}
					return foundLanguages;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rangeMinUTC"></param>
		/// <param name="maxRange"></param>
		/// <returns></returns>
		internal Invoice[] GetModifiedInvoices( DateTime ModifiedSince, DateTime maxRange )
		{
			// Create a channel factory.
			using( ChannelFactory<IInvoice> channelFactory = new ChannelFactory<IInvoice>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as
							System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IInvoice invoiceChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )invoiceChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return invoiceChan.GetInvoicesByDate( "", ModifiedSince, maxRange, 0 );
				}
			}
		}

		internal LeadSource[] GetAllLeadSources()
		{
			// Create a channel factory.
			using( ChannelFactory<ILeadSource> channelFactory = new ChannelFactory<ILeadSource>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior =
						op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as
							System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ILeadSource chan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )chan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return chan.GetAllLeadSources();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public Account[] GetModifiedAccounts( DateTime ModifiedSince )
		{
			// Create a channel factory.
			using( ChannelFactory<IAccount> channelFactory = new ChannelFactory<IAccount>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IAccount accountChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )accountChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return accountChan.GetAccountsChangedSince( ModifiedSince, 0 );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public Product[] GetModifiedProducts( DateTime ModifiedSince )
		{
			// Create a channel factory.
			using( ChannelFactory<IProduct> channelFactory = new ChannelFactory<IProduct>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IProduct productChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )productChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return productChan.GetProductsChangedSince( ModifiedSince, 0 );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public Contact[] GetModifiedContacts( DateTime ModifiedSince )
		{
			// Create a channel factory.
			using( ChannelFactory<IContact> channelFactory = new ChannelFactory<IContact>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IContact contactChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )contactChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return contactChan.GetContactsChangedSince( "", ModifiedSince, 0 );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public Case[] GetModifiedCases( DateTime ModifiedSince )
		{
			// Create a channel factory.
			using( ChannelFactory<ICase> channelFactory = new ChannelFactory<ICase>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ICase caseChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )caseChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return caseChan.GetCasesChangedSince( "", ModifiedSince, 0 );
				}
			}
		}

		public Case[] GetAllCases(  )
		{
			// Create a channel factory.
			using( ChannelFactory<ICase> channelFactory = new ChannelFactory<ICase>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ICase caseChan = channelFactory.CreateChannel();
				using( new OperationContextScope( (IContextChannel) caseChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return caseChan.GetCasesChangedSince( "", COMMON_MIN_RANGE, 0 );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public Address[] GetModifiedAddresses( DateTime ModifiedSince )
		{
			// Create a channel factory.
			using( ChannelFactory<IAddress> channelFactory = new ChannelFactory<IAddress>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IAddress addressChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )addressChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return addressChan.GetAddressesChangedSince( "", ModifiedSince, 0 );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public User[] GetModifiedUsers( DateTime ModifiedSince )
		{
			// Create a channel factory.
			using( ChannelFactory<IUser> channelFactory = new ChannelFactory<IUser>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IUser userChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )userChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return userChan.GetUsersChangedSince( "", ModifiedSince, 0 );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public WorkType[] GetAllWorkTypes()
		{
			// Create a channel factory.
			using( ChannelFactory<IWorkType> channelFactory = new ChannelFactory<IWorkType>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IWorkType worktypeChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )worktypeChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return worktypeChan.GetAllWorkTypes();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public BusinessUnit[] GetModifiedBusinessUnits( DateTime ModifiedSince )
		{
			// Create a channel factory.
			using( ChannelFactory<IBusinessUnit> channelFactory = new ChannelFactory<IBusinessUnit>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IBusinessUnit businessunitChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )businessunitChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return businessunitChan.GetBusinessUnitsChangedSince( "", ModifiedSince, 0 );
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public Company[] GetModifiedCompanies( DateTime ModifiedSince )
		{
			List<Account> allAccounts = new List<Account>();
			List<Company> allCompanies = new List<Company>();

			// Retrieving companies is a bit trickier than most of the objects.
			// First, we'll have to retrieve all the accounts and then the corresponding
			// companies one by one by their GUIDs.

			// First, create a channel factory for accounts.
			using( ChannelFactory<IAccount> channelFactory = new ChannelFactory<IAccount>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create a channel and read all the accounts into a list.
				IAccount accountChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )accountChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );

					foreach( Account _account in accountChan.GetAccountsChangedSince( ModifiedSince, 0 ) )
					{
						allAccounts.Add( _account );
					}
				}
			}

			// Create another channel for companies.
			using( ChannelFactory<ICompany> channelFactory2 = new ChannelFactory<ICompany>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory2.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ICompany companyChan = channelFactory2.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )companyChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory2.Endpoint.Contract.Namespace, m_apiKey ) );

					foreach( Account _account in allAccounts )
					{
						allCompanies.Add( companyChan.GetCompanyByGUID( _account.CompanyGUID ) );
					}
					Company[] companiesArray = allCompanies.ToArray();
					return companiesArray;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Industry[] GetAllIndustries()
		{
			// Create a channel factory.
			using( ChannelFactory<IIndustry> channelFactory = new ChannelFactory<IIndustry>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IIndustry industryChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )industryChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return industryChan.GetIndustries();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public SalesStatus[] GetAllSalesStatuses()
		{
			List<Case> allCases = new List<Case>();
			List<SalesStatus> allSalesStatuses = new List<SalesStatus>();

			// Create the first channel factory (for cases).
			using( ChannelFactory<ICase> channelFactory = new ChannelFactory<ICase>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ICase caseChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )caseChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );

					foreach( Case _case in caseChan.GetCasesChangedSince( "", COMMON_MIN_RANGE, 0 ) )
					{
						allCases.Add( _case );
					}
				}
			}

			// Create the first channel factory (for sales statuses).
			using( ChannelFactory<ISalesStatus> channelFactory2 = new ChannelFactory<ISalesStatus>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory2.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ISalesStatus salesStatusChan = channelFactory2.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )salesStatusChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory2.Endpoint.Contract.Namespace, m_apiKey ) );

					foreach( Case _case in allCases )
					{
						foreach( SalesStatus _salesstatus in salesStatusChan.GetSalesStatusesForCase( _case.GUID ) )
						{

							allSalesStatuses.Add( _salesstatus );
						}
					}
					SalesStatus[] salesStatusArray = allSalesStatuses.ToArray();
					return salesStatusArray;
				}
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Phase[] GetAllPhases()
		{
			List<Case> allCases = new List<Case>();
			List<Phase> allPhases = new List<Phase>();

			// Create a channel factory.
			using( ChannelFactory<ICase> channelFactory = new ChannelFactory<ICase>( m_binding, m_endpoint ) )
			{
				// Increase the graph depth.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ICase caseChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )caseChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );

					foreach( Case _case in caseChan.GetCasesChangedSince( "", COMMON_MIN_RANGE, 0 ) )
					{
						allCases.Add( _case );
					}
				}
			}

			using( ChannelFactory<IPhase> channelFactory2 = new ChannelFactory<IPhase>( m_binding, m_endpoint ) )
			{
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory2.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				IPhase phasesChan = channelFactory2.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )phasesChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory2.Endpoint.Contract.Namespace, m_apiKey ) );

					foreach( Case _case in allCases )
					{
						foreach( Phase _phase in phasesChan.GetPhasesByCaseGUID( _case.GUID ) )
						{
							allPhases.Add( _phase );
						}

					}
					Phase[] phaseArray = allPhases.ToArray();
					return phaseArray;
				}
			}

		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public SalesProcess[] GetAllSalesProcesses()
		{

			using( ChannelFactory<ISalesProcess> channelFactory = new ChannelFactory<ISalesProcess>( m_binding, m_endpoint ) )
			{
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ISalesProcess salesProcessChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )salesProcessChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return salesProcessChan.GetAllSalesProcesses();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Tag[] GetAllTags()
		{
			Tag[] userTagsArray;
			Tag[] caseTagsArray;
			// Create a channel factory.
			using( ChannelFactory<ITag> channelFactory = new ChannelFactory<ITag>( m_binding, m_endpoint ) )
			{
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				ITag tagChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )tagChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					caseTagsArray = tagChan.GetAllCaseTags();
					userTagsArray = tagChan.GetAllUserTags();
					Tag[] combinedTagsArray = new Tag[ caseTagsArray.Length + userTagsArray.Length ];
					Array.Copy( caseTagsArray, combinedTagsArray, caseTagsArray.Length );
					Array.Copy( userTagsArray, 0, combinedTagsArray, caseTagsArray.Length, userTagsArray.Length );

					return combinedTagsArray; //tagChan.GetAllCaseTags();
				}
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="ModifiedSince"></param>
		/// <returns></returns>
		public ProductCategory[] GetModifiedProductCategories( DateTime ModifiedSince )
		{
			// Create a channel factory.
			using( ChannelFactory<IProductCategory> channelFactory = new ChannelFactory<IProductCategory>( m_binding, m_endpoint ) )
			{
				// Increase the graph size.
				foreach( System.ServiceModel.Description.OperationDescription op in channelFactory.Endpoint.Contract.Operations )
				{
					System.ServiceModel.Description.DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<System.ServiceModel.Description.DataContractSerializerOperationBehavior>() as System.ServiceModel.Description.DataContractSerializerOperationBehavior;
					if( dataContractBehavior != null )
					{
						dataContractBehavior.MaxItemsInObjectGraph = 10000000;
					}
				}
				// Create channel and retrieve product categories.
				IProductCategory pcChan = channelFactory.CreateChannel();
				using( new OperationContextScope( ( IContextChannel )pcChan ) )
				{
					OperationContext.Current.OutgoingMessageHeaders.Add(
						System.ServiceModel.Channels.MessageHeader.CreateHeader( "WebServicePassword",
						channelFactory.Endpoint.Contract.Namespace, m_apiKey ) );
					return pcChan.GetProductCategoriesChangedSince( ModifiedSince, 0 );
				}
			}

		}

		#endregion

	}
}
