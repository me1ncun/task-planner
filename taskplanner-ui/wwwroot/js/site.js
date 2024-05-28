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

function showRegistrationForm() 
{
    $(".registration-page").show();
    $(".login-page").hide();
}

function showLoginForm() 
{

    $(".login-page").show();
    $(".registration-page").hide();
}

function checkPasswordAndRegister() 
{
    var password = $("#player-password-registration").val();
    var passwordRepeat = $("#player-password-repeat").val();
    var email = $("#player-email-registration").val();

    if (password != passwordRepeat) {
        alert("Passwords do not match");
    }
    else if (password != null && password != null && passwordRepeat != null) {
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

function checkInfoAndLogin() 
 {
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
            success: function (data, textStatus, xhr) {
                // data содержит ответ от сервера
                console.log(data);
                $("#registration-reference").hide();
                $("#login-reference").hide();
                $("#logout-reference").show();
                $(".main-block").show();
                $(".login-page").hide();

                $("#users-email").text(account.Email).show();

                taskOutput();

                console.log("You are successfully authorized");

            },
            error: function (xhr, textStatus, errorThrown) {
                alert("Password or email doesnt match, try again");
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
        '<div class="task-box-name"><b>' + task.name + '</b></div>' +
        '<div class="task-box-description">' + task.description + '</div>' +
        '<div class="flex-container-buttons">' +
        '<button class="delete-task-button" onclick="deleteTask(\'' + task.id + '\')">X</button>' +
        '<button class="checked-task-button" onclick="makeTaskUndone(\'' + task.id + '\')">V</button>' +
        '</div>' +
        '</div>';

    return taskbox;
}

function createUndoneTaskBox(task) {
    var taskbox = '<div class="task-box">' +
        '<div class="task-box-name"><b>' + task.name + '</b></div>' +
        '<div class="task-box-description">' + task.description + '</div>' +
        '<div class="flex-container-buttons">' +
        '<button class="delete-task-button" onclick="deleteTask(\'' + task.id + '\')">X</button>' +
        '<button class="checked-task-button" onclick="makeTaskDone(\'' + task.id + '\')">V</button>' +
        '</div>' +
        '</div>';

    return taskbox;
}

function taskOutput() {
    $.ajax({
        url: 'http://localhost:5173/tasks',
        type: 'GET',
        contentType: 'application/json',
        success: function (data, textStatus, xhr) {
            for (var i = 0; i < data.length; i++) {
                var task = data[i];
                if (task.isDone == "true") {
                    // Concatenate task box components
                    var taskbox = createDoneTaskBox(task);

                    // Append the task boxes to a container (adjust container selector accordingly)
                    $(".tasks-list-done").append(taskbox);

                    /*    calculateDonePlus();*/

                } else if (task.isDone == "false") {
                    // Concatenate task box components
                    var taskbox = createUndoneTaskBox(task);
                    // Append the task boxes to a container (adjust container selector accordingly)
                    $(".tasks-list-new").append(taskbox);

                    /*calculateNewPlus();*/
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
    
    var task = {
        "Title": $(".task-name").val(),
        "Description": $(".task-description").val(),
        "Status": taskResult,
    };

    $.ajax({
        url: 'http://localhost:5173/tasks',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(task),
        success: function (data, textStatus, xhr) {
            console.log(data);

            task.id = data.id;

            if (task.IsDone == "true") {

                var newTaskBox = createUndoneTaskBox(task);

                // Append the new task box to the existing tasks list
                $(".tasks-list-done").append(newTaskBox);

                /* calculateDonePlus();*/

            } else {

                var newTaskBox = createDoneTaskBox(task);;

                // Append the new task box to the existing tasks list
                $(".tasks-list-new").append(newTaskBox);

                /* calculateNewPlus();*/

            }

            console.log("Your task has been created successfully");
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }
    });
}

//$(document).ready(function () {
//    // Получаем ссылку на блок и на элемент, в котором будет отображаться результат
//    var block = $(".tasks-list-new");

//    var resultElement = document.getElementById("calculatorNew");

//    // Получаем количество элементов в блоке
//    var count = block.children(".task-box").length;
//    console.log(count);

//    // Выводим результат
//    resultElement.textContent += count;
//});

//function calculateDonePlus{

//    var resultElement = document.getElementById("calculatorDone");

//    resultElement.textContent += parseInt(resultElement.textContent) + 1;
//}

//function calculateDoneMinus{

//    var resultElement = document.getElementById("calculatorDone");

//    resultElement.textContent += parseInt(resultElement.textContent) - 1;
//}

//function calculateNewPlus{

//    var resultElement = document.getElementById("calculatorNew");

//    resultElement.textContent += parseInt(resultElement.textContent) + 1;
//}

//function calculateNewMinus{

//    var resultElement = document.getElementById("calculatorNew");

//    resultElement.textContent += parseInt(resultElement.textContent) - 1;
//}


$(document).ready(function () {
    $("#logout-reference").hide();
    $(".registration-page").hide();
    $(".login-page").hide();
    $(".main-block").hide();
});