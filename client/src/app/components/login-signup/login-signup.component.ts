import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { FloatLabelType } from '@angular/material/form-field';

@Component({
  selector: 'app-login-signup',
  templateUrl: './login-signup.component.html',
  styleUrls: ['./login-signup.component.scss'],
})
export class LoginSignupComponent implements OnInit {
  hide: boolean = true;
  form: boolean = true;
  hideRequiredControl = new FormControl(false);
  floatLabelControl = new FormControl('auto' as FloatLabelType);

  loginForm!: FormGroup;
  signupForm!: FormGroup;

  isPasswordVisible: boolean = false;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });

    this.signupForm = this.fb.group({
      fullName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      mobileNumber: ['', [Validators.required, Validators.minLength(10)]],
    });
  }

  togglePasswordVisibility(): void {
    this.isPasswordVisible = !this.isPasswordVisible;  
  }

  navigateBtn(): void {
    this.form = !this.form; 
  }

  onLoginSubmit(): void {
    if (this.loginForm.valid) {
      console.log('Login Form Values:', this.loginForm.value);
    } else {
      console.log('Login form is invalid');
    }
  }

  onSignupSubmit(): void {
    if (this.signupForm.valid) {
      console.log('SignUp Form Values:', this.signupForm.value);
    } else {
      console.log('SignUp form is invalid');
    }
  }


  getFloatLabelValue(): FloatLabelType {
    return this.floatLabelControl.value || 'auto';
  }
}
