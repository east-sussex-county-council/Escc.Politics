using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using Escc.AddressAndPersonalDetails;
using Microsoft.ApplicationBlocks.Data;

namespace Escc.Politics
{
    /// <summary>
    /// Convert councillor and committee information between database records and object model instances
    /// </summary>
    public static class PoliticsManager
    {
        #region Common data formats
        /// <summary>
        /// Builds an address from a set of BS7666-compliant fields.
        /// </summary>
        /// <param name="dataRecord">The data row.</param>
        /// <param name="fieldPrefix">The field prefix.</param>
        /// <returns>A populated BS7666 address</returns>
        private static BS7666Address BuildAddressFromData(IDataRecord dataRecord, string fieldPrefix)
        {
            return new BS7666Address(
                dataRecord[fieldPrefix + "PAON"] != DBNull.Value ? dataRecord[fieldPrefix + "PAON"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "SAON"] != DBNull.Value ? dataRecord[fieldPrefix + "SAON"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "StreetDescriptor"] != DBNull.Value ? dataRecord[fieldPrefix + "StreetDescriptor"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "Locality"] != DBNull.Value ? dataRecord[fieldPrefix + "Locality"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "Town"] != DBNull.Value ? dataRecord[fieldPrefix + "Town"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "AdministrativeArea"] != DBNull.Value ? dataRecord[fieldPrefix + "AdministrativeArea"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "Postcode"] != DBNull.Value ? dataRecord[fieldPrefix + "Postcode"].ToString().Trim() : String.Empty);
        }

        /// <summary>
        /// Builds an address from a set of BS7666-compliant fields.
        /// </summary>
        /// <param name="dataRecord">The data row.</param>
        /// <param name="fieldPrefix">The field prefix.</param>
        /// <returns>A populated BS7666 address</returns>
        private static BS7666Address BuildAddressFromData(DataRow dataRecord, string fieldPrefix)
        {
            return new BS7666Address(
                dataRecord[fieldPrefix + "PAON"] != DBNull.Value ? dataRecord[fieldPrefix + "PAON"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "SAON"] != DBNull.Value ? dataRecord[fieldPrefix + "SAON"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "StreetDescriptor"] != DBNull.Value ? dataRecord[fieldPrefix + "StreetDescriptor"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "Locality"] != DBNull.Value ? dataRecord[fieldPrefix + "Locality"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "Town"] != DBNull.Value ? dataRecord[fieldPrefix + "Town"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "AdministrativeArea"] != DBNull.Value ? dataRecord[fieldPrefix + "AdministrativeArea"].ToString().Trim() : String.Empty,
                dataRecord[fieldPrefix + "Postcode"] != DBNull.Value ? dataRecord[fieldPrefix + "Postcode"].ToString().Trim() : String.Empty);
        }

        /// <summary>
        /// Builds the parameters for saving a BS7666 address
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="parameterPrefix">The parameter prefix, eg &quot;@&quot;</param>
        private static void BuildAddressParameters(BS7666Address address, List<SqlParameter> parameters, string parameterPrefix)
        {
            parameters.Add(new SqlParameter(parameterPrefix + "SAON", SqlDbType.VarChar, 100));
            if (address != null) parameters[parameters.Count - 1].Value = address.Saon;
            parameters.Add(new SqlParameter(parameterPrefix + "PAON", SqlDbType.VarChar, 100));
            if (address != null) parameters[parameters.Count - 1].Value = address.Paon;
            parameters.Add(new SqlParameter(parameterPrefix + "StreetDescriptor", SqlDbType.VarChar, 100));
            if (address != null) parameters[parameters.Count - 1].Value = address.StreetName;
            parameters.Add(new SqlParameter(parameterPrefix + "Locality", SqlDbType.VarChar, 35));
            if (address != null) parameters[parameters.Count - 1].Value = address.Locality;
            parameters.Add(new SqlParameter(parameterPrefix + "Town", SqlDbType.VarChar, 30));
            if (address != null) parameters[parameters.Count - 1].Value = address.Town;
            parameters.Add(new SqlParameter(parameterPrefix + "AdministrativeArea", SqlDbType.VarChar, 30));
            if (address != null) parameters[parameters.Count - 1].Value = address.AdministrativeArea;
            parameters.Add(new SqlParameter(parameterPrefix + "Postcode", SqlDbType.VarChar, 8));
            if (address != null) parameters[parameters.Count - 1].Value = address.Postcode;
        }
        #endregion // Common data formats

        #region Committees
        /// <summary>
        /// Gets a committee matching the supplied id
        /// </summary>
        /// <param name="committeeId">The committee id</param>
        /// <returns>The committee, or <c>null</c> if not found</returns>
        public static Committee ReadCommittee(int committeeId)
        {
            DataSet data = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString, CommandType.StoredProcedure, "usp_Committee_SelectById", new SqlParameter("@committeeId", committeeId));
            if (data.Tables.Count == 0) return null;
            Collection<Committee> committees = BuildCommitteesFromData(data.Tables[0]);
            return (committees.Count > 0) ? committees[0] : null;
        }

        /// <summary>
        /// Gets a committee matching the supplied name
        /// </summary>
        /// <param name="committeeName">The committee name</param>
        /// <returns>The committee, or <c>null</c> if not found</returns>
        public static Committee ReadCommittee(string committeeName)
        {
            DataSet data = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString, CommandType.StoredProcedure, "usp_Committee_SelectByName", new SqlParameter("@committeeName", committeeName));
            if (data.Tables.Count == 0) return null;
            Collection<Committee> committees = BuildCommitteesFromData(data.Tables[0]);
            return (committees.Count > 0) ? committees[0] : null;
        }

        /// <summary>
        /// Reads details of all committees, or committees of a given type
        /// </summary>
        /// <param name="type">Type of committee to get</param>
        /// <returns>Committees</returns>
        public static Collection<Committee> ReadCommittees(CommitteeType type)
        {
            DataSet data = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString, CommandType.StoredProcedure, "usp_Committee_SelectByType", new SqlParameter("@committeeType", type));
            if (data.Tables.Count == 0) return new Collection<Committee>();
            return BuildCommitteesFromData(data.Tables[0]);
        }


        /// <summary>
        /// Helper method to convert the data from the raw database rows into Committee objects
        /// </summary>
        /// <param name="data">The data.</param>
        private static Collection<Committee> BuildCommitteesFromData(DataTable data)
        {
            // collection to hold the Committee ids, so we can remember what we've already done
            Collection<Committee> committees = new Collection<Committee>();
            List<string> committeesDone = new List<string>();

            // loop through data
            for (int mainRow = 0; mainRow < data.Rows.Count; mainRow++)
            {
                // for each new committee...
                if (!committeesDone.Contains(data.Rows[mainRow]["CommitteeId"].ToString()))
                {
                    // remember we've done this committee
                    committeesDone.Add(data.Rows[mainRow]["CommitteeId"].ToString());

                    // create the committee object and add the flatfile data
                    Committee committee = new Committee();
                    committee.Id = (int)data.Rows[mainRow]["CommitteeId"];
                    committee.Name = data.Rows[mainRow]["CommitteeName"].ToString();
                    if (data.Columns.Contains("Description"))
                    {
                        committee.Description = data.Rows[mainRow]["Description"].ToString();
                    }
                    if (data.Columns.Contains("MeetingVenueId") && data.Rows[mainRow]["MeetingVenueId"] != DBNull.Value)
                    {
                        committee.UsualMeetingVenue = BuildAddressFromData(data.Rows[mainRow], "Venue");
                        committee.UsualMeetingVenue.Id = (int)data.Rows[mainRow]["MeetingVenueId"];
                    }
                    if (data.Columns.Contains("Confidential"))
                    {
                        committee.Confidential = (bool)data.Rows[mainRow]["Confidential"];
                    }
                    if (data.Columns.Contains("AboutUrl") && data.Rows[mainRow]["AboutUrl"] != DBNull.Value && data.Rows[mainRow]["AboutUrl"].ToString().Length > 0)
                    {
                        committee.NavigateUrl = new Uri(data.Rows[mainRow]["AboutUrl"].ToString(), UriKind.Absolute);
                    }
                    if (data.Columns.Contains("ReportsUrl") && data.Rows[mainRow]["ReportsUrl"] != DBNull.Value && data.Rows[mainRow]["ReportsUrl"].ToString().Length > 0)
                    {
                        committee.MeetingPapersUrl = new Uri(data.Rows[mainRow]["ReportsUrl"].ToString(), UriKind.Absolute);
                    }
                    if (data.Columns.Contains("SiteUrl") && data.Rows[mainRow]["SiteUrl"] != DBNull.Value && data.Rows[mainRow]["SiteUrl"].ToString().Length > 0)
                    {
                        committee.NavigateUrlExternal = new Uri(data.Rows[mainRow]["SiteUrl"].ToString(), UriKind.Absolute);
                    }

                    // if membership was approved on a known date, parse it into a DateTime object
                    if (data.Columns.Contains("MembershipApproved") && data.Rows[mainRow]["MembershipApproved"] != null && data.Rows[mainRow]["MembershipApproved"].ToString().Length > 0)
                    {
                        committee.MembershipApproved = DateTime.Parse(data.Rows[mainRow]["MembershipApproved"].ToString(), CultureInfo.CurrentCulture);
                    }

                    BuildCommitteeCouncillorsFromData(data, mainRow, committee);

                    mainRow = BuildCommitteeMembersFromData(data, mainRow, committee);

                    // add the Committee to the collection of Committees
                    committees.Add(committee);
                }
            }

            return committees;
        }

        /// <summary>
        /// Builds the councillors on a committee from committee data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="mainRow">The main row.</param>
        /// <param name="committee">The committee.</param>
        private static void BuildCommitteeCouncillorsFromData(DataTable data, int mainRow, Committee committee)
        {
            if (data.Columns.Contains("CouncillorId"))
            {
                // Now move on to the councillors who are members of the committee.
                // They are the first cause of duplicate rows in the datatable.
                // Use this to monitor which ones have already been dealt with
                List<string> councillorsDone = new List<string>();

                // loop through councillors, but since we know all the rows for this committee are grouped together,
                // start at the current row rather than the start of the DataTable.Rows collection
                for (int subRow = mainRow; subRow < data.Rows.Count; subRow++)
                {
                    // check we're still dealing with rows for the same committee
                    if (data.Rows[subRow]["CommitteeId"].ToString() == data.Rows[mainRow]["CommitteeId"].ToString())
                    {
                        // check it's a new councillor too - there's another potential cause of duplicate rows (coming next)
                        if (data.Rows[subRow]["CouncillorId"].ToString().Length > 0 && !councillorsDone.Contains(data.Rows[subRow]["CouncillorId"].ToString()))
                        {
                            // remember we've dealt with this councillor
                            councillorsDone.Add(data.Rows[subRow]["CouncillorId"].ToString());

                            // create and populate the Councillor object
                            // Councillor inherits from CommitteeMember so they can be used together (see next bit)
                            Councillor member = new Councillor();
                            member.Id = (int)data.Rows[subRow]["CouncillorId"];
                            member.Name.GivenNames.Add(data.Rows[subRow]["FirstName"].ToString());
                            member.Name.FamilyName = data.Rows[subRow]["LastName"].ToString();
                            member.Party = new PoliticalParty(data.Rows[subRow]["Party"].ToString());
                            member.NavigateUrl = BuildCouncillorUrl(data.Rows[subRow]["UrlSegment"].ToString(), member.Id);
                            member.ElectoralDivision = new ElectoralDivision(data.Rows[subRow]["ElectoralDivision"].ToString());
                            member.ElectoralDivision.Id = Int32.Parse(data.Rows[subRow]["ElectoralDivisionId"].ToString(), CultureInfo.InvariantCulture);
                            CommitteeMembership membership = new CommitteeMembership(member);
                            membership.MemberRole = data.Rows[subRow]["CommitteeRole"].ToString();

                            // add the Councillor to the Committee
                            committee.Members.Add(membership);
                        }
                    }
                    else break; // if it's a new committee we're finished for now
                }
            }
        }

        /// <summary>
        /// Builds the meeting papers URL for a meeting.
        /// </summary>
        /// <param name="meeting">The meeting.</param>
        /// <returns></returns>
        private static Uri BuildMeetingPapersUrl(CommitteeMeeting meeting)
        {
            if (meeting == null || meeting.Committee == null || meeting.Committee.MeetingPapersUrl == null) return null;

            string meetingUrl = meeting.Committee.MeetingPapersUrl.ToString();
            string filename = Path.GetFileName(meetingUrl);
            if (filename.Length > 0 && meetingUrl.EndsWith(filename, StringComparison.CurrentCulture)) meetingUrl = meetingUrl.Substring(0, meetingUrl.Length - filename.Length);
            meetingUrl += (meeting.MeetingDate.ToString("yyyy/dMMMM", CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture) + ".htm");

            return new Uri(meetingUrl);
        }

        /// <summary>
        /// Builds the URL for a councillor's web page
        /// </summary>
        /// <param name="electoralDivisionFolder">The electoral division folder.</param>
        /// <param name="councillorId">The councillor id.</param>
        /// <returns></returns>
        private static Uri BuildCouncillorUrl(string electoralDivisionFolder, int councillorId)
        {
            return new Uri("http://www.eastsussex.gov.uk/yourcouncil/about/people/councillors/find/" + electoralDivisionFolder + "/?councillor=" + councillorId);
        }

        /// <summary>
        /// Builds committee members from committee data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="mainRow">The main row.</param>
        /// <param name="committee">The committee.</param>
        /// <returns></returns>
        private static int BuildCommitteeMembersFromData(DataTable data, int mainRow, Committee committee)
        {
            // Now loop through committee members who aren't ESCC councillors, 
            // using this to remember which ones have been dealt with
            if (data.Columns.Contains("CommitteeMemberId"))
            {
                List<string> othersDone = new List<string>();

                // Again, since we know all the rows for this committee are grouped together,
                // start at the current row
                for (int subRow = mainRow; subRow < data.Rows.Count; subRow++)
                {
                    // check we're still dealing with rows for the same committee
                    if (data.Rows[subRow]["CommitteeId"].ToString() == data.Rows[mainRow]["CommitteeId"].ToString())
                    {
                        // check it's a new committee member too - duplicate rows can be caused by councillors
                        if (data.Rows[subRow]["CommitteeMemberId"].ToString().Length > 0 && !othersDone.Contains(data.Rows[subRow]["CommitteeMemberId"].ToString()))
                        {
                            // remember we've dealt with this committee member
                            othersDone.Add(data.Rows[subRow]["CommitteeMemberId"].ToString());

                            // create and populate the CommitteeMember
                            CommitteeMember member = new CommitteeMember();
                            member.Id = (int)data.Rows[subRow]["CommitteeMemberId"];
                            member.Name.GivenNames.Add(data.Rows[subRow]["CommitteeMemberFirstName"].ToString());
                            member.Name.FamilyName = data.Rows[subRow]["CommitteeMemberLastName"].ToString();

                            // add the CommitteeMember to the Committee
                            committee.Members.Add(new CommitteeMembership(member, data.Rows[subRow]["CommitteeMemberRole"].ToString()));
                        }
                    }
                    else
                    {
                        // we know we've finished with this committee's rows, so move the main counter on a bit to save a little time
                        mainRow = subRow - 1;
                        break;
                    }
                }
            }
            return mainRow;
        }

        /// <summary>
        /// Adds or updates a committee.
        /// </summary>
        /// <param name="committee">The committee.</param>
        /// <exception cref="ArgumentNullException"><c>committee</c> cannot be <c>null</c></exception>
        public static void SaveCommittee(Committee committee)
        {
            if (committee == null) throw new ArgumentNullException("committee");

            SqlParameter[] sqlParams = new SqlParameter[10];
            sqlParams[0] = new SqlParameter("@committeeName", committee.Name);
            sqlParams[1] = new SqlParameter("@committeeType", committee.CommitteeType);
            sqlParams[2] = new SqlParameter("@confidential", committee.Confidential);
            sqlParams[3] = new SqlParameter("@description", committee.Description);
            sqlParams[4] = new SqlParameter("@aboutUrl", DBNull.Value);
            if (committee.NavigateUrl != null) sqlParams[4].Value = committee.NavigateUrl.ToString();
            sqlParams[5] = new SqlParameter("@reportsUrl", DBNull.Value);
            if (committee.MeetingPapersUrl != null) sqlParams[5].Value = committee.MeetingPapersUrl.ToString();
            sqlParams[6] = new SqlParameter("@siteUrl", DBNull.Value);
            if (committee.NavigateUrlExternal != null) sqlParams[6].Value = committee.NavigateUrlExternal.ToString();
            sqlParams[7] = new SqlParameter("@membershipApproved", committee.MembershipApproved);
            sqlParams[8] = new SqlParameter("@meetingVenueId", DBNull.Value);
            if (committee.UsualMeetingVenue != null) sqlParams[8].Value = committee.UsualMeetingVenue.Id;

            if (committee.Id > 0)
            {
                sqlParams[9] = new SqlParameter("@committeeId", committee.Id);

                SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_Committee_Update", sqlParams);
            }
            else
            {
                SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_Committee_Insert", sqlParams);
            }
        }

        /// <summary>
        /// Deletes a committee.
        /// </summary>
        /// <param name="committeeId">The committee id.</param>
        public static void DeleteCommittee(int committeeId)
        {
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_Committee_Delete", new SqlParameter("@committeeId", committeeId));
        }
        #endregion // Committees

        #region Committee meetings
        /// <summary>
        /// Deletes a meeting venue.
        /// </summary>
        /// <param name="meetingVenueId">The id of the meeting venue.</param>
        public static void DeleteMeetingVenue(int meetingVenueId)
        {
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_MeetingVenue_Delete", new SqlParameter("@meetingVenueId", meetingVenueId));
        }

        /// <summary>
        /// Reads all meeting venues 
        /// </summary>
        /// <returns>Collection of meeting venue addresses</returns>
        public static Collection<BS7666Address> ReadMeetingVenues()
        {
            Collection<BS7666Address> meetingVenues = new Collection<BS7666Address>();
            using (IDataReader reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString, CommandType.StoredProcedure, "usp_MeetingVenue_SelectAll"))
            {
                while (reader.Read())
                {
                    BS7666Address meetingVenue = BuildAddressFromData(reader, "Venue");
                    meetingVenue.Id = Int32.Parse(reader["MeetingVenueId"].ToString(), CultureInfo.InvariantCulture);
                    meetingVenues.Add(meetingVenue);
                }
            }
            return meetingVenues;
        }


        /// <summary>
        /// Reads a meeting venue.
        /// </summary>
        /// <param name="meetingVenueId">The meeting venue id.</param>
        /// <returns></returns>
        public static BS7666Address ReadMeetingVenue(int meetingVenueId)
        {
            BS7666Address meetingVenue;
            using (IDataReader reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString, CommandType.StoredProcedure, "usp_MeetingVenue_SelectById", new SqlParameter("@meetingVenueId", meetingVenueId)))
            {
                reader.Read();
                meetingVenue = BuildAddressFromData(reader, "Venue");
                meetingVenue.Id = meetingVenueId;
            }
            return meetingVenue;
        }

        /// <summary>
        /// Saves a meeting venue.
        /// </summary>
        /// <param name="venue">The venue.</param>
        public static void SaveMeetingVenue(BS7666Address venue)
        {
            if (venue == null) throw new ArgumentNullException("venue");

            List<SqlParameter> parameterList = new List<SqlParameter>();
            SqlParameter venueIdParam = new SqlParameter("@meetingVenueId", SqlDbType.Int, 4);
            parameterList.Add(venueIdParam);

            BuildAddressParameters(venue, parameterList, "@venue");

            // Convert to array
            SqlParameter[] venueParams = new SqlParameter[parameterList.Count];
            parameterList.CopyTo(venueParams);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString))
            {
                if (venue.Id > 0)
                {
                    // Update existing venue
                    venueIdParam.Value = venue.Id;

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "usp_MeetingVenue_Update", venueParams);
                }
                else
                {
                    // Add new venue
                    venueIdParam.Direction = ParameterDirection.Output;

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "usp_MeetingVenue_Insert", venueParams);

                    venue.Id = Int32.Parse(venueIdParam.Value.ToString(), CultureInfo.InvariantCulture);
                }
            }
        }


        /// <summary>
        /// Deletes a committee meeting.
        /// </summary>
        /// <param name="meetingId">The meeting id.</param>
        public static void DeleteMeeting(int meetingId)
        {
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_Meeting_Delete", new SqlParameter("@meetingId", meetingId));
        }

        /// <summary>
        /// Saves a committee meeting.
        /// </summary>
        /// <param name="meeting">The meeting.</param>
        public static void SaveMeeting(CommitteeMeeting meeting)
        {
            if (meeting == null) throw new ArgumentNullException("meeting");

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@committeeId", meeting.Committee.Id));
            parameterList.Add(new SqlParameter("@meetingDate", meeting.MeetingDate));
            parameterList.Add(new SqlParameter("@meetingVenueId", meeting.Venue.Id));
            parameterList.Add(new SqlParameter("@meetingStatus", meeting.Status.ToString()));

            // Convert to array
            SqlParameter[] parameters = new SqlParameter[parameterList.Count];
            parameterList.CopyTo(parameters);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString))
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "usp_Meeting_Insert", parameters);
            }
        }

        /// <summary>
        /// Updates the status of a committee meeting, to cancel it for example.
        /// </summary>
        /// <param name="meeting">The meeting.</param>
        public static void SaveMeetingStatus(CommitteeMeeting meeting)
        {
            if (meeting == null) throw new ArgumentNullException("meeting");

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@meetingId", meeting.MeetingId));
            parameterList.Add(new SqlParameter("@meetingStatus", meeting.Status.ToString()));

            // Convert to array
            SqlParameter[] parameters = new SqlParameter[parameterList.Count];
            parameterList.CopyTo(parameters);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString))
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "usp_Meeting_StatusUpdate", parameters);
            }
        }

        /// <summary>
        /// Reads a list of committee meetings
        /// </summary>
        /// <param name="committeeId">The committee id, or <c>null</c> for all committees</param>
        /// <param name="fromDate">if set, read only meetings on or after the given date.</param>
        public static Collection<CommitteeMeeting> ReadMeetings(int? committeeId, DateTime? fromDate)
        {
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@committeeId", committeeId));
            parameterList.Add(new SqlParameter("@fromDate", fromDate));

            // Convert to array
            SqlParameter[] parameters = new SqlParameter[parameterList.Count];
            parameterList.CopyTo(parameters);

            Collection<CommitteeMeeting> meetings = new Collection<CommitteeMeeting>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString))
            {
                using (IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, "usp_Meeting_SelectAll", parameters))
                {
                    while (reader.Read())
                    {
                        CommitteeMeeting meeting = (CommitteeMeeting)BuildMeetingFromData(reader, "Meeting", "Venue", new CommitteeMeeting());
                        meeting.Venue.Id = (int)reader["MeetingVenueId"];
                        meeting.Committee = new Committee(reader["CommitteeName"].ToString());
                        meeting.Committee.Id = Int32.Parse(reader["CommitteeId"].ToString(), CultureInfo.InvariantCulture);
                        meeting.Committee.Confidential = (bool)reader["Confidential"];
                        if (reader["AboutUrl"] != DBNull.Value && !String.IsNullOrEmpty(reader["AboutUrl"].ToString()))
                        {
                            meeting.Committee.NavigateUrl = new Uri(reader["AboutUrl"].ToString(), UriKind.Absolute);
                        }
                        if (reader["ReportsUrl"] != DBNull.Value && !String.IsNullOrEmpty(reader["ReportsUrl"].ToString()))
                        {
                            meeting.Committee.MeetingPapersUrl = new Uri(reader["ReportsUrl"].ToString(), UriKind.Absolute);

                            // if it's later than 5 days before the meeting, the committee papers should be online
                            // so build up an expected URL based on a naming convention
                            if (meeting.MeetingDate <= DateTime.Today.AddDays(-5))
                            {
                                meeting.MeetingPapersUrl = BuildMeetingPapersUrl(meeting);
                            }
                        }
                        meetings.Add(meeting);
                    }
                }
            }

            return meetings;
        }


        /// <summary>
        /// Builds the meeting from data.
        /// </summary>
        /// <param name="meetingData">The meeting data.</param>
        /// <param name="meetingFieldPrefix">The meeting field prefix.</param>
        /// <param name="venueFieldPrefix">The venue field prefix.</param>
        /// <param name="meeting">The meeting.</param>
        /// <returns></returns>
        private static Meeting BuildMeetingFromData(IDataRecord meetingData, string meetingFieldPrefix, string venueFieldPrefix, Meeting meeting)
        {
            // create and populate the meeting
            meeting.MeetingId = (int)meetingData[meetingFieldPrefix + "Id"];
            meeting.MeetingDate = (DateTime)meetingData[meetingFieldPrefix + "Date"];
            meeting.Venue = new BS7666Address(meetingData[venueFieldPrefix + "PAON"].ToString(),
                meetingData[venueFieldPrefix + "SAON"].ToString(),
                meetingData[venueFieldPrefix + "StreetDescriptor"].ToString(),
                meetingData[venueFieldPrefix + "Locality"].ToString(),
                meetingData[venueFieldPrefix + "Town"].ToString(),
                meetingData[venueFieldPrefix + "AdministrativeArea"].ToString(),
                meetingData[venueFieldPrefix + "Postcode"].ToString());
            meeting.Status = (MeetingStatus)Enum.Parse(typeof(MeetingStatus), meetingData[meetingFieldPrefix + "Status"].ToString());
            meeting.DateModified = (DateTime)meetingData["DateModified"];
            return meeting;
        }
        #endregion Committee meetings

        #region Councillors
        /// <summary>
        /// Get details for all councillors
        /// </summary>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        public static Collection<Councillor> ReadCouncillors(CouncillorSortOrder sortOrder)
        {
            // create connection & command
            SqlParameter sortParam = new SqlParameter("@sortField", SqlDbType.Int, 4);
            sortParam.Value = (int)sortOrder;

            DataSet councillorData = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString, CommandType.StoredProcedure, "usp_Councillor_SelectAll", sortParam);

            Collection<Councillor> councillors = new Collection<Councillor>();

            if (councillorData.Tables.Count > 0)
            {
                foreach (DataRow row in councillorData.Tables[0].Rows)
                {
                    // Check for null values because a division can be without councillor details if there's a by-election pending
                    Councillor cllr = new Councillor();
                    if (row["councillorid"] != DBNull.Value) cllr.Id = Int32.Parse(row["councillorid"].ToString(), CultureInfo.InvariantCulture);
                    if (row["firstname"] != DBNull.Value) cllr.Name.GivenNames.Add(row["firstname"].ToString().Trim());
                    if (row["lastname"] != DBNull.Value) cllr.Name.FamilyName = row["lastname"].ToString().Trim();
                    cllr.ElectoralDivision = new ElectoralDivision(row["electoraldivision"].ToString().Trim());
                    cllr.ElectoralDivision.FolderName = row["urlsegment"].ToString();
                    if (row["party"] != DBNull.Value) cllr.Party = new PoliticalParty(row["party"].ToString().Trim());
                    councillors.Add(cllr);
                }
            }

            // return the data
            return councillors;
        }

        /// <summary>
        /// Get details for a single councillor
        /// </summary>
        /// <param name="councillorId">The councillor id.</param>
        /// <returns></returns>
        public static Councillor ReadCouncillor(int councillorId)
        {
            // create connection & command
            SqlParameter idParam = new SqlParameter("@councillorId", SqlDbType.Int, 4);
            idParam.Value = (int)councillorId;

            Councillor councillor = null;
            using (IDataReader councillorReader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString, CommandType.StoredProcedure, "usp_SelectCouncillorById", idParam))
            {
                List<string> organisationRoles = new List<string>();
                List<string> committeeMemberships = new List<string>();
                while (councillorReader.Read())
                {
                    // Build councillor data from first row of result
                    if (councillor == null) councillor = BuildCouncillorFromData(councillorReader);

                    // Gather status from each row of data
                    if (councillorReader["status"] != DBNull.Value && !councillor.CouncilStatus.Contains(councillorReader["status"].ToString().Trim()) && councillorReader["status"].ToString().Trim().Length > 0)
                    {
                        councillor.CouncilStatus.Add(councillorReader["status"].ToString().Trim());
                    }

                    // Gather committee membership from each row
                    if (councillorReader["CommitteeId"] != DBNull.Value && !committeeMemberships.Contains(councillorReader["CommitteeId"].ToString()))
                    {
                        CommitteeMembership committee = new CommitteeMembership();
                        committee.Committee = new Committee(councillorReader["CommitteeName"].ToString());
                        committee.Committee.Id = Int32.Parse(councillorReader["CommitteeId"].ToString(), CultureInfo.InvariantCulture);
                        committee.MemberRole = councillorReader["CommitteeRole"].ToString();
                        committee.Committee.Confidential = (bool)councillorReader["Confidential"];
                        if (councillorReader["AboutUrl"] != DBNull.Value && councillorReader["AboutUrl"].ToString().Length > 0)
                        {
                            committee.Committee.NavigateUrl = new Uri(councillorReader["AboutUrl"].ToString(), UriKind.Absolute);
                        }
                        if (councillorReader["ReportsUrl"] != DBNull.Value && councillorReader["ReportsUrl"].ToString().Length > 0)
                        {
                            committee.Committee.MeetingPapersUrl = new Uri(councillorReader["ReportsUrl"].ToString(), UriKind.Absolute);
                        }
                        councillor.Committees.Add(committee);

                        committeeMemberships.Add(councillorReader["CommitteeId"].ToString());
                    }

                    // Gather affiliations from each row
                    if (councillorReader["OutsideOrganisationId"] != DBNull.Value)
                    {
                        string organisationRole = councillorReader["OutsideOrganisationId"].ToString();
                        if (councillorReader["Role"] != DBNull.Value) organisationRole += councillorReader["Role"].ToString();
                        if (!organisationRoles.Contains(organisationRole))
                        {
                            CouncillorAffiliation affiliation = new CouncillorAffiliation();
                            affiliation.OrganisationId = Int32.Parse(councillorReader["OutsideOrganisationId"].ToString(), CultureInfo.InvariantCulture);
                            affiliation.OrganisationName = councillorReader["OrganisationName"].ToString().Trim();
                            affiliation.Councillor = councillor;
                            if (councillorReader["Role"] != DBNull.Value) affiliation.CouncillorRole = councillorReader["Role"].ToString().Trim();
                            councillor.Affiliations.Add(affiliation);

                            organisationRoles.Add(organisationRole);
                        }
                    }
                }
            }

            // return the data
            return councillor;
        }

        /// <summary>
        /// Gets the given name, family name and image URL for a councillor
        /// </summary>
        /// <param name="councillorId">The councillor id.</param>
        /// <returns></returns>
        public static Councillor ReadCouncillorSummary(int councillorId)
        {
            SqlParameter prm = new SqlParameter("@councillorId", SqlDbType.Int, 4);
            prm.Value = councillorId;

            Councillor councillor = new Councillor();
            using (SqlDataReader r = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString, CommandType.StoredProcedure, "usp_SelectCouncillorSummaryById", prm))
            {
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        councillor.Id = councillorId;
                        councillor.Name.GivenNames.Add(r["FirstName"].ToString());
                        councillor.Name.FamilyName = r["LastName"].ToString();
                        councillor.Party = new PoliticalParty(r["Party"].ToString());
                        string imageUrl = r["Image"].ToString();
                        if (imageUrl.Length > 0) councillor.ImageUrl = new Uri(imageUrl, UriKind.Absolute);
                    }
                }
            }
            return councillor;
        }


        /// <summary>
        /// Builds the councillor from data.
        /// </summary>
        /// <param name="dataRecord">The data record.</param>
        /// <returns></returns>
        private static Councillor BuildCouncillorFromData(IDataRecord dataRecord)
        {
            Councillor councillor = new Councillor();
            councillor.Id = Int32.Parse(dataRecord["councillorid"].ToString(), CultureInfo.InvariantCulture);
            councillor.Name.GivenNames.Add(dataRecord["firstname"].ToString().Trim());
            if (dataRecord["MiddleNames"] != null && dataRecord["MiddleNames"] != DBNull.Value) councillor.Name.GivenNames.Add(dataRecord["MiddleNames"].ToString());
            councillor.Name.FamilyName = dataRecord["lastname"].ToString().Trim();
            if (dataRecord["party"] != DBNull.Value) councillor.Party = new PoliticalParty(dataRecord["party"].ToString().Trim());
            if (dataRecord["electoraldivision"] != DBNull.Value)
            {
                councillor.ElectoralDivision = new ElectoralDivision(dataRecord["electoraldivision"].ToString().Trim());
                councillor.ElectoralDivision.Id = Int32.Parse(dataRecord["ElectoralDivisionId"].ToString(), CultureInfo.InvariantCulture);
            }
            if (dataRecord["DateElected"] != DBNull.Value) councillor.DateElected = DateTime.Parse(dataRecord["DateElected"].ToString(), CultureInfo.CurrentCulture);
            if (dataRecord["Image"] != DBNull.Value)
            {
                string councillorImage = dataRecord["image"].ToString().Trim();
                if (!String.IsNullOrEmpty(councillorImage)) councillor.ImageUrl = new Uri(councillorImage, UriKind.RelativeOrAbsolute);
            }
            if (dataRecord["interests"] != DBNull.Value) councillor.Interests = dataRecord["interests"].ToString().Trim();
            if (dataRecord["UrlSegment"] != DBNull.Value) councillor.NavigateUrl = BuildCouncillorUrl(dataRecord["UrlSegment"].ToString(), councillor.Id);


            BuildCouncillorContactsFromData(dataRecord, councillor);

            return councillor;
        }

        /// <summary>
        /// Builds a councillor's contact details from data.
        /// </summary>
        /// <param name="dataRecord">The data record.</param>
        /// <param name="councillor">The councillor.</param>
        private static void BuildCouncillorContactsFromData(IDataRecord dataRecord, Councillor councillor)
        {
            if (dataRecord["HomePhone"] != DBNull.Value)
            {
                UKContactNumber homePhone = new UKContactNumber(dataRecord["HomePhone"].ToString());
                homePhone.Usage = ContactUsage.Home;
                councillor.TelephoneNumbers.Add(homePhone);
            }
            if (dataRecord["WorkPhone"] != DBNull.Value)
            {
                UKContactNumber workPhone = new UKContactNumber(dataRecord["WorkPhone"].ToString());
                workPhone.Usage = ContactUsage.Work;
                councillor.TelephoneNumbers.Add(workPhone);
            }
            if (dataRecord["MobilePhone"] != DBNull.Value)
            {
                UKContactNumber mobilePhone = new UKContactNumber(dataRecord["MobilePhone"].ToString());
                mobilePhone.Mobile = true;
                councillor.TelephoneNumbers.Add(mobilePhone);
            }
            if (dataRecord["Fax"] != DBNull.Value) councillor.FaxNumbers.Add(dataRecord["Fax"].ToString());
            if (dataRecord["email"] != DBNull.Value) councillor.EmailAddresses.Add(dataRecord["email"].ToString().Trim());
            if (dataRecord["Url"] != DBNull.Value)
            {
                string councillorPage = dataRecord["url"].ToString().Trim();
                if (!String.IsNullOrEmpty(councillorPage)) councillor.PersonalWebsiteUrl = new Uri(councillorPage, UriKind.RelativeOrAbsolute);
            }
            councillor.HomeAddress = BuildAddressFromData(dataRecord, "Home");
            councillor.WorkAddress = BuildAddressFromData(dataRecord, "Work");
        }

        /// <summary>
        /// Deletes a councillor.
        /// </summary>
        /// <param name="councillorId">The councillor id.</param>
        public static void DeleteCouncillor(int councillorId)
        {
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_DeleteMember", new SqlParameter("@idMember", councillorId));
        }

        /// <summary>
        /// Saves a councillor.
        /// </summary>
        /// <param name="councillor">The councillor.</param>
        public static void SaveCouncillor(Councillor councillor)
        {
            if (councillor == null) throw new ArgumentNullException("councillor");

            List<SqlParameter> councillorParameterList = new List<SqlParameter>();
            SqlParameter councillorIdParam = new SqlParameter("@councillorId", DBNull.Value);
            councillorParameterList.Add(councillorIdParam);

            BuildCouncillorPersonalParameters(councillor, councillorParameterList);
            BuildCouncillorContactParameters(councillor, councillorParameterList);

            // Convert to array
            SqlParameter[] councillorParams = new SqlParameter[councillorParameterList.Count];
            councillorParameterList.CopyTo(councillorParams);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString))
            {
                if (councillor.Id > 0)
                {
                    // Update existing councillor
                    councillorIdParam.Value = councillor.Id;

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "usp_Councillor_Update", councillorParams);
                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "usp_Councillor_Status_Delete", councillorIdParam);
                }
                else
                {
                    // Add new councillor
                    councillorIdParam.Size = 4;
                    councillorIdParam.Direction = ParameterDirection.Output;

                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "usp_Councillor_Insert", councillorParams);

                    councillor.Id = Int32.Parse(councillorIdParam.Value.ToString(), CultureInfo.InvariantCulture);
                }

                // Either way, add statuses to councillor
                foreach (string status in councillor.CouncilStatus)
                {
                    SqlParameter[] statusParams = new SqlParameter[2];
                    statusParams[0] = new SqlParameter("@councillorId", councillor.Id);
                    statusParams[1] = new SqlParameter("@status", status);
                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "usp_Councillor_Status_Insert", statusParams);
                }
            }
        }

        /// <summary>
        /// Builds the parameters for adding or updating a councillor.
        /// </summary>
        /// <param name="councillor">The councillor.</param>
        /// <param name="councillorParams">The councillor params.</param>
        private static void BuildCouncillorPersonalParameters(Councillor councillor, List<SqlParameter> councillorParams)
        {
            councillorParams.Add(new SqlParameter("@firstName", councillor.Name.GivenNames[0].ToString()));
            councillorParams.Add(new SqlParameter("@middleNames", (councillor.Name.GivenNames.Count > 1) ? councillor.Name.GivenNames.ToString().Substring(councillor.Name.GivenNames[0].Length + 1) : String.Empty));
            councillorParams.Add(new SqlParameter("@lastName", councillor.Name.FamilyName));
            var party = new SqlParameter("@party", DBNull.Value);
            if (councillor.Party != null) party.Value = councillor.Party.Name;
            councillorParams.Add(party);
            councillorParams.Add(new SqlParameter("@electoralDivisionId", councillor.ElectoralDivision.Id));
            councillorParams.Add(new SqlParameter("@image", DBNull.Value));
            if (councillor.ImageUrl != null) councillorParams[councillorParams.Count - 1].Value = councillor.ImageUrl.ToString();
            councillorParams.Add(new SqlParameter("@dateElected", DBNull.Value));
            if (councillor.DateElected != null) councillorParams[councillorParams.Count - 1].Value = councillor.DateElected.Value;
            councillorParams.Add(new SqlParameter("@interests", DBNull.Value));
            if (!String.IsNullOrEmpty(councillor.Interests)) councillorParams[councillorParams.Count - 1].Value = councillor.Interests;
        }

        /// <summary>
        /// Builds the parameters for adding or updating a councillor's contact details.
        /// </summary>
        /// <param name="councillor">The councillor.</param>
        /// <param name="councillorParams">The councillor params.</param>
        private static void BuildCouncillorContactParameters(Councillor councillor, List<SqlParameter> councillorParams)
        {
            BuildAddressParameters(councillor.HomeAddress, councillorParams, "@home");
            BuildAddressParameters(councillor.WorkAddress, councillorParams, "@work");
            councillorParams.Add(new SqlParameter("@email", DBNull.Value));
            if (councillor.EmailAddresses.Count > 0) councillorParams[councillorParams.Count - 1].Value = councillor.EmailAddresses[0].ToString();
            councillorParams.Add(new SqlParameter("@url", DBNull.Value));
            if (councillor.PersonalWebsiteUrl != null) councillorParams[councillorParams.Count - 1].Value = councillor.PersonalWebsiteUrl.ToString();
            SqlParameter homePhone = new SqlParameter("@homePhone", DBNull.Value);
            councillorParams.Add(homePhone);
            SqlParameter workPhone = new SqlParameter("@workPhone", DBNull.Value);
            councillorParams.Add(workPhone);
            SqlParameter mobilePhone = new SqlParameter("@mobilePhone", DBNull.Value);
            councillorParams.Add(mobilePhone);
            foreach (UKContactNumber phoneNumber in councillor.TelephoneNumbers)
            {
                if (phoneNumber.Usage == ContactUsage.Home && !phoneNumber.Mobile) homePhone.Value = phoneNumber.NationalNumber;
                else if (phoneNumber.Usage == ContactUsage.Work && !phoneNumber.Mobile) workPhone.Value = phoneNumber.NationalNumber;
                else if (phoneNumber.Mobile) mobilePhone.Value = phoneNumber.NationalNumber;
            }
            councillorParams.Add(new SqlParameter("@fax", DBNull.Value));
            if (councillor.FaxNumbers.Count > 0) councillorParams[councillorParams.Count - 1].Value = councillor.FaxNumbers[0].NationalNumber;
        }

        #endregion // Councillors

        #region Committee memberships
        /// <summary>
        /// Adds a councillor to a committee.
        /// </summary>
        /// <param name="membership">The membership.</param>
        /// <exception cref="ArgumentNullException"><c>membership</c> cannot be <c>null</c></exception>
        public static void AddCouncillorToCommittee(CommitteeMembership membership)
        {
            if (membership == null) throw new ArgumentNullException("membership");

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_Councillor_Committee_Insert",
                                        new SqlParameter("@councillorId", membership.Member.Id),
                                        new SqlParameter("@committeeId", membership.Committee.Id),
                                        new SqlParameter("@committeeRole", membership.MemberRole));
        }
        /// <summary>
        /// Removes a councillor from a committee.
        /// </summary>
        /// <param name="membership">The membership.</param>
        /// <exception cref="ArgumentNullException"><c>membership</c> cannot be <c>null</c></exception>
        public static void DeleteCouncillorFromCommittee(CommitteeMembership membership)
        {
            if (membership == null) throw new ArgumentNullException("membership");

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_Councillor_Committee_Delete",
                                        new SqlParameter("@councillorId", membership.Member.Id),
                                        new SqlParameter("@committeeId", membership.Committee.Id));
        }


        /// <summary>
        /// Adds a committee member to a committee.
        /// </summary>
        /// <param name="membership">The membership.</param>
        /// <exception cref="ArgumentNullException"><c>membership</c> cannot be <c>null</c></exception>
        public static void AddCommitteeMemberToCommittee(CommitteeMembership membership)
        {
            if (membership == null) throw new ArgumentNullException("membership");

            object profileUrl = DBNull.Value;
            if (membership.Member.NavigateUrl != null) profileUrl = membership.Member.NavigateUrl.ToString();

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_CommitteeMember_Insert",
                                        new SqlParameter("@committeeId", membership.Committee.Id),
                                        new SqlParameter("@firstName", membership.Member.Name.GivenNames.ToString()),
                                        new SqlParameter("@lastName", membership.Member.Name.FamilyName),
                                        new SqlParameter("@profileUrl", profileUrl),
                                        new SqlParameter("@committeeRole", membership.MemberRole));
        }
        /// <summary>
        /// Removes a committee member from a committee.
        /// </summary>
        /// <param name="membership">The membership.</param>
        /// <exception cref="ArgumentNullException"><c>membership</c> cannot be <c>null</c></exception>
        public static void DeleteCommitteeMemberFromCommittee(CommitteeMembership membership)
        {
            if (membership == null) throw new ArgumentNullException("membership");

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_CommitteeMember_Delete",
                                        new SqlParameter("@committeeMemberId", membership.Member.Id));
        }
        #endregion // Committee memberships

        #region Councillor affiliations
        /// <summary>
        /// Deletes a councillor's affiliation to an organisation other than East Sussex County Council.
        /// </summary>
        /// <param name="affiliation">The affiliation.</param>
        /// <exception cref="ArgumentNullException"><c>affiliation</c> cannot be <c>null</c></exception>
        public static void DeleteCouncillorAffiliation(CouncillorAffiliation affiliation)
        {
            if (affiliation == null) throw new ArgumentNullException("affiliation");

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_Councillor_OutsideOrganisation_Delete",
                                        new SqlParameter("@councillorId", affiliation.Councillor.Id),
                                        new SqlParameter("@outsideOrganisationId", affiliation.OrganisationId));
        }

        /// <summary>
        /// Adds a councillor's affiliation to an organisation other than East Sussex County Council.
        /// </summary>
        /// <param name="affiliation">The affiliation.</param>
        /// <exception cref="ArgumentNullException"><c>affiliation</c> cannot be <c>null</c></exception>
        public static void AddCouncillorAffiliation(CouncillorAffiliation affiliation)
        {
            if (affiliation == null) throw new ArgumentNullException("affiliation");

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_Councillor_OutsideOrganisation_Insert",
                                        new SqlParameter("@councillorId", affiliation.Councillor.Id),
                                        new SqlParameter("@organisationName", affiliation.OrganisationName),
                                        new SqlParameter("@councillorRole", affiliation.CouncillorRole));
        }
        #endregion // Councillor affiliations

        #region Electoral divisions
        /// <summary>
        /// Gets all electoral divisions
        /// </summary>
        /// <returns></returns>
        public static Collection<ElectoralDivision> ReadElectoralDivisions()
        {
            Collection<ElectoralDivision> divisions = new Collection<ElectoralDivision>();
            using (IDataReader divisionReader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString, CommandType.StoredProcedure, "usp_ElectoralDivision_SelectAll"))
            {
                while (divisionReader.Read())
                {
                    ElectoralDivision division = new ElectoralDivision();
                    division.Id = Int32.Parse(divisionReader["ElectoralDivisionId"].ToString(), CultureInfo.InvariantCulture);
                    division.Name = divisionReader["ElectoralDivision"].ToString();
                    divisions.Add(division);
                }
            }
            return divisions;
        }

        /// <summary>
        /// Gets a list of parishes in a given electoral division
        /// </summary>
        /// <param name="electoralDivisionId">The electoral division to get parishes for</param>
        /// <returns></returns>
        public static Collection<Parish> ReadParishesInElectoralDivision(int electoralDivisionId)
        {
            // create SQL parameter to pass in id
            SqlParameter param = new SqlParameter("@electoralDivisionId", SqlDbType.Int, 4);
            param.Value = electoralDivisionId;

            // create connection & command
            Collection<Parish> parishes = new Collection<Parish>();
            using (IDataReader reader = SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString, CommandType.StoredProcedure, "usp_ParishSelectByElectoralDivision", param))
            {
                while (reader.Read())
                {
                    Parish parish = new Parish();
                    parish.ParishName = reader["ParishName"].ToString();
                    parish.PartiallyRepresented = (bool)reader["PartialCoverage"];
                    parishes.Add(parish);
                }
            }
            return parishes;
        }

        #endregion // Electoral divisions

        #region Surgeries

        /// <summary>
        /// Deletes a councillor's surgery date.
        /// </summary>
        /// <param name="surgeryId">The surgery id.</param>
        public static void DeleteSurgery(int surgeryId)
        {
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString, CommandType.StoredProcedure, "usp_Surgery_Delete", new SqlParameter("@surgeryId", surgeryId));
        }

        /// <summary>
        /// Saves a councillor's surgery date.
        /// </summary>
        /// <param name="surgery">The surgery.</param>
        public static void SaveSurgery(Surgery surgery)
        {
            if (surgery == null) throw new ArgumentNullException("surgery");

            List<SqlParameter> parameterList = new List<SqlParameter>();

            SqlParameter surgeryId = new SqlParameter("@surgeryId", SqlDbType.Int, 4);
            surgeryId.Direction = ParameterDirection.Output;
            parameterList.Add(surgeryId);

            parameterList.Add(new SqlParameter("@councillorId", surgery.Councillor.Id));
            parameterList.Add(new SqlParameter("@surgeryDate", surgery.MeetingDate));
            parameterList.Add(new SqlParameter("@surgeryStatus", surgery.Status.ToString()));
            BuildAddressParameters(surgery.Venue, parameterList, "surgery");

            SqlParameter shortcut = new SqlParameter("@copyAddressFromSurgeryId", DBNull.Value);
            if (surgery.Venue.Id > 0) shortcut.Value = surgery.Venue.Id;
            parameterList.Add(shortcut);

            // Convert to array
            SqlParameter[] parameters = new SqlParameter[parameterList.Count];
            parameterList.CopyTo(parameters);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString))
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "usp_Surgery_Insert", parameters);
                surgery.MeetingId = Convert.ToInt32(surgeryId.Value, CultureInfo.InvariantCulture);
            }
        }


        /// <summary>
        /// Updates the status of a surgery, to cancel it for example.
        /// </summary>
        /// <param name="surgery">The surgery.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SaveSurgeryStatus(Surgery surgery)
        {
            if (surgery == null) throw new ArgumentNullException("surgery");

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@surgeryId", surgery.MeetingId));
            parameterList.Add(new SqlParameter("@surgeryStatus", surgery.Status.ToString()));

            // Convert to array
            SqlParameter[] parameters = new SqlParameter[parameterList.Count];
            parameterList.CopyTo(parameters);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Escc.Politics.ReadWrite"].ConnectionString))
            {
                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "usp_Surgery_StatusUpdate", parameters);
            }
        }

        /// <summary>
        /// Reads a list of a councillor's surgery dates
        /// </summary>
        /// <param name="councillorId">The councillor id</param>
        /// <param name="fromDate">if set, read only surgeries on or after the given date.</param>
        public static Collection<Surgery> ReadSurgeries(int? councillorId, DateTime? fromDate)
        {
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@councillorId", councillorId));
            parameterList.Add(new SqlParameter("@fromDate", fromDate));

            // Convert to array
            SqlParameter[] parameters = new SqlParameter[parameterList.Count];
            parameterList.CopyTo(parameters);

            Collection<Surgery> surgeries = new Collection<Surgery>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Escc.Politics.Read"].ConnectionString))
            {
                using (IDataReader reader = SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, "usp_Surgery_SelectByCouncillor", parameters))
                {
                    while (reader.Read())
                    {
                        Surgery surgery = (Surgery)BuildMeetingFromData(reader, "Surgery", "Surgery", new Surgery());
                        surgery.Councillor = new Councillor();
                        surgery.Councillor.Id = Int32.Parse(reader["councillorid"].ToString(), CultureInfo.InvariantCulture);
                        surgery.Councillor.Name.GivenNames.Add(reader["firstname"].ToString().Trim());
                        surgery.Councillor.Name.FamilyName = reader["lastname"].ToString().Trim();
                        surgery.Councillor.NavigateUrl = BuildCouncillorUrl(reader["UrlSegment"].ToString(), surgery.Councillor.Id);

                        surgeries.Add(surgery);
                    }
                }
            }

            return surgeries;
        }


        #endregion
    }
}
