function makeTaskDone(taskId) {
    getTaskById(taskId)
        .then(function (task) {

            var copiedTask = Object.assign({}, task);
            copiedTask.id = parseInt(taskId) + 1;
            copiedTask.isDone = "true";

            var element = document.getElementById('users-email');

            copiedTask.AuthorEmail = element.innerText;


            createTaskByTask(copiedTask);

            deleteTask(taskId);

            //calculateNewMinus();
            //calculateDonePlus();

            var taskbox = createDoneTaskBox(copiedTask);

            // Append the task boxes to a container (adjust container selector accordingly)
            $(".tasks-list-done").append(taskbox);
        })
        .catch(function (error) {
            console.error('Error:', error);
        });
}

function makeTaskUndone(taskId) {
    getTaskById(taskId)
        .then(function (task) {
            task.isDone = true;  // Assuming IsDone is a boolean

            var copiedTask = Object.assign({}, task);
            copiedTask.id = parseInt(taskId) + 1;
            copiedTask.isDone = "false";
            //

            var element = document.getElementById('users-email');
            var value = element.innerText; // или element.textContent;
            //
            copiedTask.AuthorEmail = value;
            //

            createTaskByTask(copiedTask);

            deleteTask(taskId);

            //calculateDoneMinus();
            //calculateNewPlus();

            var taskbox = createUndoneTaskBox(copiedTask);

            // Append the task boxes to a container (adjust container selector accordingly)
            $(".tasks-list-new").append(taskbox);
        })
        .catch(function (error) {
            console.error('Error:', error);
        });
}


function createTaskByTask(task) {

    var task = {
        "id": task.id,
        "name": task.name,
        "description": task.description,
        "isDone": task.isDone,
        "authorEmail": task.AuthorEmail
    };

    $.ajax({
        url: 'https://localhost:7201/task',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(task),
        success: function (data, textStatus, xhr) {

            console.log("Your task has been created successfully");
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }
    });
}

function showRegistrationForm() {
    $(".registration-page").show();
    $(".login-page").hide();
    $(".reset-page").hide();
}

function showLoginForm() {
    $(".reset-page").hide();
    $(".login-page").show();
    $(".registration-page").hide();
}

function checkPasswordAndRegister() {
    var password = $("#player-password-registration").val();
    var passwordRepeat = $("#player-password-repeat").val();
    var email = $("#player-email-registration").val();

    if (password != passwordRepeat) {
        alert("Passwords do not match");
    } else if (password != null && password != null && passwordRepeat != null) {
        event.preventDefault()
        var account = {
            "email": email,
            "password": password
        };
        $.ajax({
            url: 'http://localhost:5173/user',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(account),
            success: function (data, textStatus, xhr) {
                console.log(data);
                console.log("Your account has been created successfully");
                $("#logout-reference").show();
                $(".registration-page").hide();
                $("#login-reference").hide();
                $("#registration-reference").hide();
                $(".main-block").show();


                $("#users-email").text(account.Email);
                $("#users-email").show();

            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
            }
        });
    }
}

function checkInfoAndLogin() {
    event.preventDefault();
    var password = $("#player-password-login").val();
    var email = $("#player-email-login").val();

    if (password != null && email != null) {

        var account = {
            "email": email,
            "password": password,
            "repeatPassword": password
        };

        $.ajax({
            url: 'http://localhost:5173/auth/login',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(account),
            xhrFields: {
                withCredentials: true // Включает куки в запрос
            },
            success: function (data, textStatus, xhr) {
                // data содержит ответ от сервера
                console.log(data);
                $("#registration-reference").hide();
                $("#login-reference").hide();
                $("#logout-reference").show();
                $(".main-block").show();
                $(".login-page").hide();

                $("#users-email").text(account.email).show();

                taskOutput();

                console.log("You are successfully authorized");

            },
            error: function (xhr, textStatus, errorThrown) {
                alert("Password or email doesnt match, try again");
            }
        });
    }
}

function showResetPage() {
    $(".reset-page").show();
    $(".login-page").hide();
}

function resetPassword() {
    var newPassword = $("#player-password-reset").val();
    var confirmPassword = $("#player-password-repeat-reset").val();
    var email = $("#player-email-login").val();

    if (newPassword != confirmPassword) {
        alert("Passwords do not match");
    } else if (newPassword != null && confirmPassword != null) {
        event.preventDefault()
        var account = {
            "email": email,
            "newPassword": newPassword,
            "confirmPassword": confirmPassword
        };
        $.ajax({
            url: 'http://localhost:5173/user/reset-password',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(account),
            success: function (data, textStatus, xhr) {
                console.log("Your password has been changed");

                $(".reset-page").hide();
                $(".login-page").show();

            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
            }
        });
    }
}

function getTaskById(taskId) {
    var task = {
        "Id": parseInt(taskId)
    };

    return new Promise(function (resolve, reject) {
        $.ajax({
            url: 'https://localhost:7201/task/' + task.Id,
            type: 'GET',
            contentType: 'application/x-www-form-urlencoded',
            success: function (data, textStatus, xhr) {
                // Обработка успешного запроса
                console.log('Task:', data);
                resolve(data); // Резолвим Promise с полученными данными
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
                reject(errorThrown); // Реджектим Promise с ошибкой
            }
        });
    });
};

function deleteTask(taskId) {
    var task = {
        "Id": parseInt(taskId)
    };
    var requestData = $.param(task);

    $.ajax({
        url: 'https://localhost:7201/task?' + requestData, // добавляем параметры в URL
        type: 'DELETE',
        contentType: 'application/x-www-form-urlencoded',
        success: function (textStatus, xhr) {
            deleteAllTasks();

            /*$(".flex-item").css("height", "-=105px");*/ //

            console.log("Your task has been deleted successfully");
            taskOutput();

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }
    });
}

function deleteAllTasks() {
    // Очистка содержимого контейнера с выполненными задачами
    $(".tasks-list-done").empty();

    // Очистка содержимого контейнера с новыми задачами
    $(".tasks-list-new").empty();
}


function createDoneTaskBox(task) {
    var taskbox = '<div class="task-box">' +
        '<div class="task-box-name"><b>' + task.title + '</b></div>' +
        '<div class="task-box-description">' + task.description + '</div>' +
        '<div class="flex-container-buttons">' +
        '<img src="photo/bin.png" style="width:20px; height: 20px; cursor: pointer; padding-right: 4px;"></img>' +
        '<img src="photo/plus-button.png" style="width:22px; height: 22px; cursor: pointer;"></img>' +
        '</div>' +
        '</div>';

    return taskbox;
}

function createUndoneTaskBox(task) {
    var taskbox = '<div class="task-box">' +
        '<div class="task-box-name"><b>' + task.title + '</b></div>' +
        '<div class="task-box-description">' + task.description + '</div>' +
        '<div class="flex-container-buttons">' +
        '<img src="photo/bin.png" style="width:20px; height: 20px; cursor: pointer; padding-right: 4px;"></img>' +
        '<img src="photo/plus-button.png" style="width:22px; height: 22px; cursor: pointer;"></img>' +
        '</div>' +
        '</div>';

    return taskbox;
}

function taskOutput() {
    $.ajax({
        url: 'http://localhost:5173/tasks',
        type: 'GET',
        contentType: 'application/json',
        xhrFields: {
            withCredentials: true // Включает куки в запрос
        },
        success: function (data, textStatus, xhr) {
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
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }
    });
}

function logOut() {
    $("#logout-reference").hide();
    $("#login-reference").show();
    $("#registration-reference").show();
    $(".main-block").hide();
    $("#users-email").hide();
    location.reload();
}

function createTask() {
    let taskResult = $("#task-isdone").prop("checked") ? "Done" : "Not done";
    var title = $(".task-name").val();
    var description = $(".task-description").val()

    var task = {
        "Title": title,
        "Description": description,
        "Status": taskResult,
    };

    $.ajax({
        url: 'http://localhost:5173/tasks',
        type: 'POST',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: new URLSearchParams({
            'title': title,
            'Description': description,
            'Status': taskResult
        })
            .toString(),
        xhrFields: {
            withCredentials: true
        },
        success: function (data, textStatus, xhr) {
            console.log(data);

            if (taskResult == "Done") {
                var newTaskBox = createUndoneTaskBox(task);

                $(".tasks-list-done").append(newTaskBox);

            } else {
                var newTaskBox = createDoneTaskBox(task);
                ;

                $(".tasks-list-new").append(newTaskBox);
            }

            console.log("Your task has been created successfully");
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }
    });
}

$(document).ready(function () {
    $("#logout-reference").hide();
    $(".registration-page").hide();
    $(".login-page").hide();
    $(".main-block").hide();
});