const hostApi = 'http://localhost:8080';
$(document).ready(function () {

    var loginForm = $(".login-page");
    var registerForm = $(".registration-page");
    var resetPassForm = $(".reset-page");
    var registerRef = $("#registration-reference");
    var loginRef = $("#login-reference");
    var logoutRef = $("#logout-reference");
    var mainPage = $(".main-block");
    var usersEmail = $("#users-email");
    var resetPassRef = $("#reset-reference");
    var loginButton = $(".login-button");
    var registerButton = $(".register-button");
    var createTaskButton = $("#create-task-button");
    var resetPassButton = $("#reset-password-button");
    var error = $(".error-message");

    async function fetchData() {
        try {
            const response = await fetch(hostApi + '/user', {
                credentials: 'include'
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();

            console.log(data);
            if (data.email != null && data.email != "") {
                usersEmail.text(data.email).show();
                registerRef.hide();
                loginRef.hide();
                logoutRef.show();
                mainPage.show();
                registerForm.hide();
                loginForm.hide();
                tasksOutput();
            } else {
                usersEmail.hide();
                registerRef.show();
                loginRef.show();
                logoutRef.hide();
                mainPage.hide();
            }
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    function showForm(formToShow) {
        // Скрываем все формы
        $(loginForm).hide();
        $(registerForm).hide();
        $(resetPassForm).hide();

        // Показываем нужную форму
        $(formToShow).show();
    }

    $(registerRef).click(function () {
        showForm('.registration-page');
    });

    $(loginRef).click(function () {
        showForm('.login-page');
    });

    $(resetPassRef).click(function () {
        showForm('.reset-page');
    })

    $(loginButton).click(function () {
        logIn();
    });

    $(registerButton,).click(function () {
        register();
    });

    $(logoutRef).click(function () {
        logOut();
    });

    $(createTaskButton).click(function () {
        createTask();
    });

    $(resetPassButton).click(function () {
        resetPassword();
    });


    function createTask() {
        var title = $(".task-name").val();
        var description = $(".task-description").val();
        var isDone = $("#task-isdone").prop("checked");
        var status = isDone ? "Done" : "Not done";

        var task = {
            "title": title,
            "description": description,
            "status": status
        };

        $.ajax({
            url: hostApi + '/task',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(task),
            xhrFields: {
                withCredentials: true
            },
            success: function () {
                console.log("Your task has been created successfully");
                $(".task-name").val("");
                $(".task-description").val("");
                fetchData();
            },
            error: function () {
                console.log("Error in Operation");
            }
        });
    };

    function logOut() {
        $.ajax({
            url: hostApi + '/logout',
            type: 'POST',
            xhrFields: {
                withCredentials: true
            },
            success: function () {
                console.log("You have successfully logged out");
                fetchData();
            },
            error: function () {
                console.log("Error in Operation");
            }
        })
    };

    function logIn() {
        var email = $("#player-email-login").val();
        var password = $("#player-password-login").val();
        if (password == null || email == null) {
            alert('Data cannot be empty');
        }

        var account = {
            "email": email,
            "password": password
        };

        $.ajax({
            url: hostApi + '/auth/login',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(account),
            xhrFields: {
                withCredentials: true
            },
            success: function () {
                console.log("You are successfully authorized" + " " + account.email);
                fetchData();
            },
            error: function (textStatus) {
                $(error).text(formatErrorMessage(textStatus));
            }
        });
    };

    function formatErrorMessage(textStatus) {
        if (textStatus.status === 0) {
            return ('Not connected.\nPlease verify your network connection.');
        } else if (textStatus.status === 404) {
            return ('User not found, try again.');
        } else if (textStatus.status === 401) {
            return ('Sorry!! You session has expired. Please login to continue access.');
        } else if (textStatus.status === 500) {
            return ('Internal Server Error.');
        } else if (textStatus.status === 409) {
            return ('Password does not match');
        } else {
            return ('Unknown error occured. Please try again.');
        }
    }

    function register() {
        var email = $("#player-email-registration").val();
        var password = $("#player-password-registration").val();
        var repeatPassword = $("#player-password-repeat-registration").val();

        var account = {
            "email": email,
            "password": password,
            "confirmPassword": repeatPassword
        };

        $.ajax({
            url: hostApi + '/auth/register',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(account),
            xhrFields: {
                withCredentials: true
            },
            success: function () {
                console.log("Your account has been created successfully");
                fetchData();
            },
            error: function (textStatus) {
                $(error).text(formatErrorMessage(textStatus));
            }
        });
    }

    function resetPassword() {
        var forgottenPassword = $("#player-password-forgotten").val();
        var newPassword = $("#player-password-reset").val();
        var confirmPassword = $("#player-password-repeat-reset").val();
        var email = $("#player-email-reset").val();

        if (newPassword != confirmPassword) {
            console.log("Passwords do not match");
        }
        var account = {
            "forgottenPassword": forgottenPassword,
            "email": email,
            "newPassword": newPassword,
            "confirmPassword": confirmPassword
        };
        $.ajax({
            url: 'http://localhost:8080/user/reset-password',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(account),
            success: function () {
                resetPassForm.hide();
                console.log("Your password has been changed");
            },
            error: function (textStatus) {
                $(error).text(formatErrorMessage(textStatus));
            }
        });
    }

    fetchData();
})
;

function swithTaskStatus(task) {
    var taskDecoded = JSON.parse(decodeURIComponent(task));
    var status = taskDecoded.status == "Done" ? "Not done" : "Done";
    taskDecoded.status = status;

    $.ajax({
        url: hostApi + "/task",
        type: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(taskDecoded),
        xhrFields: {
            withCredentials: true
        },
        success: function () {
            console.log("Task status has been switched successfully");
            tasksOutput();
        },
        error: function (texStatus) {
            console.log("Error in Operation");
        }
    });
}

function editTask(task) {
    $('#myModal').modal('show');

    var taskDecoded = JSON.parse(decodeURIComponent(task));
    console.log(taskDecoded);

    $('#save-update').on('click', function () {
        if ($('#task-name').val() == "" || $('#task-description').val() == "") {
            alert('Data cannot be empty');
        }

        var id = taskDecoded.id;
        var title = $('#task-title-update').val();
        var description = $('#task-description-update').val();
        var isDone = $('#task-isdone-update').prop("checked");
        var status = isDone ? "Done" : "Not done";

        var task = {
            "id": id,
            "title": title,
            "description": description,
            "status": status,
            "doneAt": taskDecoded.doneAt
        };

        $.ajax({
            url: hostApi + "/task/" + task.id,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(task),
            xhrFields: {
                withCredentials: true
            },
            success: function () {
                $('#myModal').modal('hide');
                console.log("Task was updated successfully");
                tasksOutput();
            },
            error: function (texStatus) {
                console.log("Error in Operation");
            }
        });
    });
}

function tasksOutput() {
    $.ajax({
        url: hostApi + '/tasks',
        type: 'GET',
        xhrFields: {
            withCredentials: true
        },
        success: function (data) {
            // Clear lists before adding new tasks
            $(".tasks-list-done").empty();
            $(".tasks-list-new").empty();
            for (var i = 0; i < data.length; i++) {
                var task = data[i];
                if (task.status == "Done") {
                    var taskbox = createDoneTaskBox(task);

                    $(".tasks-list-done").append(taskbox);

                } else if (task.status == "Not done") {
                    var taskbox = createUndoneTaskBox(task);

                    $(".tasks-list-new").append(taskbox);
                }
            }
            calcNewTasks();
            calcDoneTasks();
        },
        error: function () {
            console.log('Error in Operation');
        }
    });
}

function createDoneTaskBox(task) {
    var taskJs = encodeURIComponent(JSON.stringify(task));
    var taskbox = '<div class="task-box">' +
        '<div class="task-box-name"><b>' + task.title + '</b></div>' +
        '<div class="task-box-description">' + task.description + '</div>' +
        '<div class="flex-container-buttons">' +
        '<img src="icons/edit.png" style="width:20px; height: 20px; cursor: pointer;" id="edit-task" onclick="editTask(\'' + taskJs + '\')"></img>' +
        '<img src="icons/bin.png" style="width:20px; height: 20px; cursor: pointer;" id="delete-button" onclick="deleteTask(\'' + task.id + '\')"></img>' +
        '<img src="icons/plus-button.png" style="width:22px; height: 22px; cursor: pointer;" onclick="swithTaskStatus(\'' + taskJs + '\')"></img>' +
        '</div>' +
        '</div>';

    return taskbox;
}

function createUndoneTaskBox(task) {
    var taskJs = encodeURIComponent(JSON.stringify(task));
    var taskbox = '<div class="task-box">' +
        '<div class="task-box-name"><b>' + task.title + '</b></div>' +
        '<div class="task-box-description">' + task.description + '</div>' +
        '<div class="flex-container-buttons">' +
        '<img src="icons/edit.png" style="width:20px; height: 20px; cursor: pointer;" id="edit-task" onclick="editTask(\'' + taskJs + '\')"></img>' +
        '<img src="icons/bin.png" style="width:20px; height: 20px; cursor: pointer;" id="delete-button" onClick="deleteTask(\'' + task.id + '\')"></img>' +
        '<img src="icons/plus-button.png" style="width:22px; height: 22px; cursor: pointer;" onclick="swithTaskStatus(\'' + taskJs + '\')"></img>' +
        '</div>' +
        '</div>';

    return taskbox;
}

function calcNewTasks() {
    var length = $('.tasks-list-new > div').length;
    $("#calculatorNew").text(length);
}

function calcDoneTasks() {
    var length = $('.tasks-list-done > div').length;
    $("#calculatorDone").text(length);
}

function deleteTask(taskId) {
    $.ajax({
        url: hostApi + '/task/' + taskId,
        type: 'DELETE',
        xhrFields: {
            withCredentials: true
        },
        success: function () {
            tasksOutput();
            console.log("Your task has been deleted successfully");
        },
        error: function () {
            console.log("Error in Operation");
        }
    });
}
