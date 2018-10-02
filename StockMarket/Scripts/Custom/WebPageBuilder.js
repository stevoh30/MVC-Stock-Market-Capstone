
// These javascript methods validate user input by verifying 
// that they are in correct format. They are called using the 
// onclick button action in the form.

function validate_register() {
    var fname = document.getElementById("name1").value;
    var Lname = document.getElementById("name2").value;
    var username = document.getElementById("name3").value;

    var email = document.getElementById("email1").value;
    var val = new Boolean(validate_Email(email));
    var password = document.getElementById("pass1").value;
    var confirmpassword = document.getElementById("pass2").value;
    if ((fname == "") && (Lname == "") && (username == "") && (val == false) && (password == "") && (confirmpassword == "") && (password != confirmpassword)) {
        alert("Register was unsuccessful, please check that are fields are filled out");
    }
}
function validate_Email(email) {
    var email_check = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return email_check.test(email);
}

function validate_login() {
    var username = document.getElementById("name1").value;
    var password = document.getElementById("pass1").value;
    if (username == "" && password == "") {
        alert("Login was unsuccessful, please check your username and password");
    }
}
