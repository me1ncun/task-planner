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

    $(registerButton).click(function () {
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
            error: function () {
                console.log("Password or email doesnt match, try again");
            }
        });
    };

    function register() {
        var email = $("#player-email-registration").val();
        var password = $("#player-password-registration").val();
        var repeatPassword = $("#player-password-repeat-registration").val();

        if (password != repeatPassword) {
            console.log("Passwords do not match");
        }

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
            error: function () {
                console.log("Error in Operation");
            }
        });
    }

    function resetPassword() {
        var newPassword = $("#player-password-reset").val();
        var confirmPassword = $("#player-password-repeat-reset").val();
        var email = $("#player-email-reset").val();

        if (newPassword != confirmPassword) {
            console.log("Passwords do not match");
        }
        var account = {
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
            error: function () {
                console.log('Error in Operation');
            }
        });
    }

    function editTask(taskId) {
        // code
    }

    function swithTaskStatus(task) {
        $.ajax({
            url: hostApi + "/task",
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(task),
            xhrFields: {
                withCredentials: true
            },
            success: function () {
                console.log("Task status has been switched successfully");
                fetchData();
            },
            error: function () {
                console.log("Error in Operation");
            }
        });
    }


    fetchData();
})
;

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
    var taskbox = '<div class="task-box">' +
        '<div class="task-box-name"><b>' + task.title + '</b></div>' +
        '<div class="task-box-description">' + task.description + '</div>' +
        '<div class="flex-container-buttons">' +
        '<img src="icons/edit.png" style="width:20px; height: 20px; cursor: pointer;" onclick="editTask(\'' + task.id + '\')"></img>' +
        '<img src="icons/bin.png" style="width:20px; height: 20px; cursor: pointer;" id="delete-button" onclick="deleteTask(\'' + task.id + '\')"></img>' +
        '<img src="icons/plus-button.png" style="width:22px; height: 22px; cursor: pointer;"></img>' +
        '</div>' +
        '</div>';

    return taskbox;
}

function createUndoneTaskBox(task) {
    var taskbox = '<div class="task-box">' +
        '<div class="task-box-name"><b>' + task.title + '</b></div>' +
        '<div class="task-box-description">' + task.description + '</div>' +
        '<div class="flex-container-buttons">' +
        '<img src="icons/edit.png" style="width:20px; height: 20px; cursor: pointer;" onclick="editTask(\'' + task.id + '\')"></img>' +
        '<img src="icons/bin.png" style="width:20px; height: 20px; cursor: pointer;" id="delete-button" onClick="deleteTask(\'' + task.id + '\')"></img>' +
        '<img src="icons/plus-button.png" style="width:22px; height: 22px; cursor: pointer;"></img>' +
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

/*
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

function editTask(taskId) {
    var task = {
        "Id": parseInt(taskId)
    };

    openModal();

    $.ajax({
        url: 'http://localhost:8080/tasks',
        type: 'PUT',
        data: new URLSearchParams({
            'id': task.Id,
        }).toString(),
        contentType: 'application/x-www-form-urlencoded',
        xhrFields: {
            withCredentials: true // Включает куки в запрос
        },
        success: function (textStatus, xhr) {

            console.log("Your task has been deleted successfully");
            taskOutput();

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }
    });
}


function openModal() {
    const backdrop = document.querySelector('#modal-backdrop');
    document.addEventListener('click', modalHandler);

    function modalHandler(evt) {
        const modalBtnOpen = evt.target.closest('.js-modal');
        if (modalBtnOpen) { // open btn click
            const modalSelector = modalBtnOpen.dataset.modal;
            showModal(document.querySelector(modalSelector));
        }

        const modalBtnClose = evt.target.closest('.modal-close');
        if (modalBtnClose) { // close btn click
            evt.preventDefault();
            hideModal(modalBtnClose.closest('.modal-window'));
        }

        if (evt.target.matches('#modal-backdrop')) { // backdrop click
            hideModal(document.querySelector('.modal-window.show'));
        }
    }

    function showModal(modalElem) {
        modalElem.classList.add('show');
        backdrop.classList.remove('hidden');
    }

    function hideModal(modalElem) {
        modalElem.classList.remove('show');
        backdrop.classList.add('hidden');
    }
};*/
