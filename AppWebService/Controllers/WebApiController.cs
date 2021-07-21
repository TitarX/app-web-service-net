using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Text;
using System.Text.RegularExpressions;
using NHibernate;
using NHibernate.Linq;
using AppWebService.Models;

namespace AppWebService.Controllers
{
    public class WebApiController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Object> Get()
        {
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    List<Applications> applications = session.Query<Applications>().ToList();

                    // Сортировка по убыванию номеров заявок
                    applications.Sort(delegate(Applications application1, Applications application2) {
                        if (application1.NumberApplication < application2.NumberApplication)
                        {
                            return 1;
                        }
                        else if (application1.NumberApplication > application2.NumberApplication)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    });

                    return applications;
                }
            }
            catch (Exception e)
            {
                List<String> response = new List<String>();
                response.Add("Error");
                response.Add(e.Message);
                return response;
            }
        }

        // GET api/<controller>/5
        public IEnumerable<Object> Get(int id)
        {
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    Applications applications = session.Get<Applications>(id);
                    session.Flush();

                    List<Applications> response = new List<Applications>();
                    response.Add(applications);
                    return response;
                }
            }
            catch (Exception e)
            {
                List<String> response = new List<String>();
                response.Add("Error");
                response.Add(e.Message);
                return response;
            }
        }

        // POST api/<controller>
        public IEnumerable<Object> Post(Applications applications)
        {
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    if(String.IsNullOrWhiteSpace(applications.CompanyName))
                    {
                        stringBuilder.Append("<br />Значение поля \"Наименование организации\" отсутствует");
                    }
                    if (String.IsNullOrWhiteSpace(applications.UserName))
                    {
                        stringBuilder.Append("<br />Значение поля \"ФИО пользователя\" отсутствует");
                    }
                    if (String.IsNullOrWhiteSpace(applications.Position))
                    {
                        stringBuilder.Append("<br />Значение поля \"Должность\" отсутствует");
                    }
                    if (String.IsNullOrWhiteSpace(applications.Email))
                    {
                        stringBuilder.Append("<br />Значение поля \"Email\" отсутствует");
                    }
                    else if (!Regex.IsMatch(applications.Email, "^(?:[A-Za-z0-9](?:\\.?[A-Za-z0-9]+)*)@(?:[A-Za-z0-9](?:\\-?[A-Za-z0-9]+)+)\\.(?:[A-Za-z]{2,6})$"))
                    {
                        stringBuilder.Append("<br />Значение поля \"Email\" имеет недопустимый формат");
                    }
                    String stringBuilderResult = stringBuilder.ToString();

                    if(String.IsNullOrWhiteSpace(stringBuilderResult))
                    {
                        List<String> response = new List<String>();
                        response.Add("Success");

                        if (applications.NumberApplication == 0)
                        {
                            applications.DateApplication = DateTime.Now.ToString("dd.MM.yyyy");
                            session.Save(applications);
                            response.Add("Add");
                        }
                        else
                        {
                            Applications newApplication = session.Get<Applications>(applications.NumberApplication);
                            session.Flush();

                            newApplication.NumberApplication = applications.NumberApplication;
                            newApplication.CompanyName = applications.CompanyName;
                            newApplication.UserName = applications.UserName;
                            newApplication.Position = applications.Position;
                            newApplication.Email = applications.Email;
                            session.Update(newApplication, applications.NumberApplication);

                            response.Add("Update");
                            response.Add(applications.NumberApplication.ToString());
                        }
                        
                        session.Flush();
                        return response;
                    }
                    else
                    {
                        List<String> response = new List<String>();
                        response.Add("Error");

                        if (applications.NumberApplication == 0)
                        {
                            response.Add(new StringBuilder("Заявка не добавлена<br />").Append(stringBuilderResult).ToString());
                        }
                        else
                        {
                            response.Add(new StringBuilder("Заявка не изменена<br />").Append(stringBuilderResult).ToString());
                        }

                        return response;
                    }
                }
            }
            catch (Exception e)
            {
                List<String> response = new List<String>();
                response.Add("Error");
                response.Add(e.Message);
                return response;
            }
        }

        // DELETE api/<controller>/5
        public IEnumerable<Object> Delete(int id)
        {
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    Applications applications = new Applications();
                    applications.NumberApplication = id;

                    session.Delete(applications);
                    session.Flush();

                    List<String> response = new List<String>();
                    response.Add("Success");
                    return response;
                }
            }
            catch (Exception e)
            {
                List<String> response = new List<String>();
                response.Add("Error");
                response.Add(e.Message);
                return response;
            }
        }
    }
}
