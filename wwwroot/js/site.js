const container = document.getElementById('container');
const registerBtn = document.getElementById('register');
const loginBtn = document.getElementById('login');
const signUpForm = document.querySelector('.sign-up');
const signInForm = document.querySelector('.sign-in');

registerBtn.addEventListener('click', () => {
    container.classList.add("active");
    signUpForm.style.display = 'block';
    signInForm.style.display = 'none';
});

loginBtn.addEventListener('click', () => {
    container.classList.remove("active");
    signInForm.style.display = 'block';
    signUpForm.style.display = 'none';
});
document.addEventListener('DOMContentLoaded', function () {
    const roleSelect = document.querySelector('select[name="Role"]');
    const farmerFields = document.getElementById('farmer-fields');
    const employeeFields = document.getElementById('employee-fields');

    function toggleRoleFields() {
        if (roleSelect.value === 'farmer') {
            farmerFields.style.display = 'block';
            employeeFields.style.display = 'none';
        } else if (roleSelect.value === 'employee') {
            farmerFields.style.display = 'none';
            employeeFields.style.display = 'block';
        } else {
            farmerFields.style.display = 'none';
            employeeFields.style.display = 'none';
        }
    }

    roleSelect.addEventListener('change', toggleRoleFields);
    toggleRoleFields();
});



document.getElementById('signup-form').addEventListener('submit', function (e) {
    e.preventDefault();
    // Client-side validation if needed
    this.submit();
});

document.getElementById('signin-form').addEventListener('submit', function (e) {
    e.preventDefault();
    // Client-side validation if needed
    this.submit();
});

