var model = {
    applications: ko.observableArray(),
    modal: {
        title: ko.observable(),
        message: ko.observable()
    },
    add: {
        number: ko.observable(),
        company: ko.observable(),
        user: ko.observable(),
        position: ko.observable(),
        email: ko.observable(),
        title: ko.observable(),
        button: ko.observable(),
        isreopen: ko.observable()
    },
    del: {
        id: ko.observable()
    }
};

function webApiRequest(method, callback, param, data)
{
    jQuery.ajax("/api/webapi" + (param ? "/" + param : ""), {
        type: method,
        success: callback,
        data: data
    });
}

function getApplications()
{
    webApiRequest("GET", function (data) {
        if (data[0] === "Error")
        {
            model.modal.title("Ошибка");
            model.modal.message(data[1]);
            model.add.isreopen("");
            jQuery("#alert-modal").modal("show");
        }
        else
        {
            model.applications.removeAll();

            for (var i = 0; i < data.length; i++)
            {
                model.applications.push(data[i]);
            }
        }
    });
}

function changeApplication(item)
{
    webApiRequest(
        "GET",
        function (data) {
            if (data[0] === "Error")
            {
                model.modal.title("Ошибка");
                model.modal.message(data[1]);
                model.add.isreopen("");
                jQuery("#alert-modal").modal("show");
            }
            else
            {
                model.add.number(data[0].NumberApplication);
                model.add.company(data[0].CompanyName);
                model.add.user(data[0].UserName);
                model.add.position(data[0].Position);
                model.add.email(data[0].Email);

                model.add.title("Изменение заявки №" + item.NumberApplication);
                model.add.button("Изменить");
                model.add.isreopen("-1");
                jQuery("#add-modal").modal("show");
            }
        },
        item.NumberApplication
    );
}

function addApplication()
{
    model.add.number("");
    model.add.company("");
    model.add.user("");
    model.add.position("");
    model.add.email("");

    model.add.title("Добавление новой заявки");
    model.add.button("Добавить");
    model.add.isreopen("-1");
    jQuery("#add-modal").modal("show");
}

function addApplicationRequest()
{
    webApiRequest(
        "POST",
        function (data) {
            if (data[0] === "Error")
            {
                model.modal.title("Ошибка");
                model.modal.message(data[1]);
                jQuery("#alert-modal").modal("show");
            }
            else if (data[0] === "Success")
            {
                model.modal.title("Успех");

                if (data[1] === "Add")
                {
                    model.modal.message("Заявка успешно добавлена");
                }
                else if(data[1] === "Update")
                {
                    model.modal.message("Заявка №" + data[2] + " успешно изменена");
                }

                model.add.isreopen("");
                jQuery("#alert-modal").modal("show");

                getApplications();
            }
            else
            {
                model.modal.title("Предупреждение");
                model.modal.message("Неизвестный ответ сервиса");
                jQuery("#alert-modal").modal("show");
            }
        },
        null,
        {
            NumberApplication: model.add.number,
            CompanyName: model.add.company,
            UserName: model.add.user,
            Position: model.add.position,
            Email: model.add.email
        }
    );
}

function deleteApplication(item)
{
    model.del.id(item.NumberApplication);
    jQuery("#delete-modal").modal("show");
}

function deleteApplicationRequest()
{
    webApiRequest(
        "DELETE",
        function (data) {
            if (data[0] === "Error")
            {
                model.modal.title("Ошибка");
                model.modal.message(data[1]);
                model.add.isreopen("");
                jQuery("#alert-modal").modal("show");
            }
            else if (data[0] === "Success")
            {
                model.modal.title("Успех");
                model.modal.message("Заявка успешно удалена");
                model.add.isreopen("");
                jQuery("#alert-modal").modal("show");

                getApplications();
            }
            else
            {
                model.modal.title("Предупреждение");
                model.modal.message("Неизвестный ответ сервиса");
                model.add.isreopen("");
                jQuery("#alert-modal").modal("show");
            }
        },
        model.del.id()
    );
}

function reOpenApplicationWindow()
{
    if (model.add.isreopen() == "-1")
    {
        jQuery("#add-modal").modal("show");
    }
}

$(document).ready(function ()
{
    getApplications();
    ko.applyBindings(model);
});
