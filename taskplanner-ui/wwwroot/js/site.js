const hostApi = 'http://localhost:8080';

$(document).ready(function () {

/*   class User{
        constructor(id, email){
            this.id = id;
            this.email = email;
        }
    }
    
    var user;
    
    var getUser = getAuthorizedUser().then(function (data) {
         user = new User(data.id, data.email);
         
         console.log(user);
    });
    
    function getAuthorizedUser(){
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: hostApi + '/user',
                type: 'GET',
                contentType: 'application/json',
                xhrFields: {
                    withCredentials: true
                },
                success: function (data, textStatus, xhr) {
                  /!*  console.log(data);*!/
                   resolve(data); 
                },
                error: function (xhr, textStatus, errorThrown) {
                    console.log('Error in Operation');
                    reject(errorThrown);
                }
            });
        });
    }*/
    
    async function fetchData(){
        const response = await fetch(hostApi + '/user');
        const data = await response.json();
        console.log(data);
    }
});
//     var loginForm = $(".login-page");
//     var registerForm = $(".registration-page");
//     var resetPassForm = $(".reset-page");
//     var registerRef = $("#registration-reference");
//     var loginRef = $("#login-reference");
//     var logoutRef = $("#logout-reference");
//     var mainPage = $(".main-block");
//     var usersEmail = $("#users-email");

//     if (getJwtFromLocalStorage != null || getJwtFromLocalStorage !== '') {
//         logoutRef.show();
//         mainPage.show();
//     }
//     else {
//         registerRef.show();
//         loginRef.show();
//         logoutRef.hide();
//         mainPage.hide();
//     }

//     function logIn() {
//         var password = $("#player-password-login").val();
//         var email = $("#player-email-login").val();

//         if (password == null || email == null) {
//             alert('Data cannot be empty');
//         }
//         var account = {
//             "email": email,
//             "password": password,
//             "repeatPassword": password
//         };

//         $.ajax({
//             url: hostApi + '/auth/login',
//             type: 'POST',
//             contentType: 'application/json',
//             data: JSON.stringify(account),
//             xhrFields: {
//                 withCredentials: true
//             },
//             success: function () {


//                 // $("#registration-reference").hide();
//                 // $("#login-reference").hide();
//                 // $("#logout-reference").show();
//                 // $(".main-block").show();
//                 // $(".login-page").hide();

//                 usersEmail.text(account.email).show();

//                 taskOutput();

//                 alert("You are successfully authorized");
//             },
//             error: function () {
//                 alert("Password or email doesnt match, try again");
//             }
//         });
//     }

//     function showRegistrationForm() {
//         registerForm.show();
//         loginForm.hide();
//         resetPassForm.hide();
//     }

//     function showLoginForm() {
//         resetPassForm.hide();
//         loginForm.show();
//         registerForm.hide();
//     }

//     function getJwtFromLocalStorage() {
//         return localStorage.getItem("token")
//     }
// });

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
            url: 'http://localhost:8080/user',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(account),
            xhrFields: {
                withCredentials: true // Включает куки в запрос
            },
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
            url: 'http://localhost:8080/auth/login',
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
            url: 'http://localhost:8080/user/reset-password',
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

    $.ajax({
        url: 'http://localhost:8080/tasks',
        type: 'DELETE',
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

function createDoneTaskBox(task) {
    var taskbox = '<div class="task-box">' +
        '<div class="task-box-name"><b>' + task.title + '</b></div>' +
        '<div class="task-box-description">' + task.description + '</div>' +
        '<div class="flex-container-buttons">' +
        '<img src="icons/edit.png" style="width:20px; height: 20px; cursor: pointer;" onclick="editTask(\'' + task.id + '\')"></img>' +
        '<img src="icons/bin.png" style="width:20px; height: 20px; cursor: pointer;" onclick="deleteTask(\'' + task.id + '\')"></img>' +
        '<img src="icons/plus-button.png" style="width:22px; height: 22px; cursor: pointer;"></img>' +
        '</div>' +
        '</div>';

    return taskbox;
}

function calculateNew(){
    var length = $('.tasks-list-new > div').length;
    $("#calculatorNew").text(length);
}

function calculateDone(){
    var length = $('.tasks-list-done > div').length;
    $("#calculatorDone").text(length);
}

function createUndoneTaskBox(task) {
    var taskbox = '<div class="task-box">' +
        '<div class="task-box-name"><b>' + task.title + '</b></div>' +
        '<div class="task-box-description">' + task.description + '</div>' +
        '<div class="flex-container-buttons">' +
        '<img src="icons/edit.png" style="width:20px; height: 20px; cursor: pointer;" onclick="editTask(\'' + task.id + '\')"></img>' +
        '<img src="icons/bin.png" style="width:20px; height: 20px; cursor: pointer;" onclick="deleteTask(\'' + task.id + '\')"></img>' +
        '<img src="icons/plus-button.png" style="width:22px; height: 22px; cursor: pointer;"></img>' +
        '</div>' +
        '</div>';

    return taskbox;
}

function taskOutput() {
    $.ajax({
        url: 'http://localhost:8080/tasks',
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
            calculateNew();
            calculateDone();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }
    });
}

function logOut() {
    $.ajax({
        url: 'http://localhost:8080/logout',
        type: 'POST',
        xhrFields: {
            withCredentials: true // Включает куки в запрос
        },
        success: function (data, textStatus, xhr) {
            console.log("You have successfully loged out");
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }
    });

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
    var description = $(".task-description").val();
    
    if(description == ""){
        description = "No description";
    }

    var task = {
        "Title": title,
        "Description": description,
        "Status": taskResult,
    };

    $.ajax({
        url: 'http://localhost:8080/tasks',
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
};