import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { DataService } from 'src/app/Services/dataService/data.service';
import { UserService } from 'src/app/Services/userService/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  token: string | null = null;
  profileForm!: FormGroup;
  subscription!: Subscription;
  userProfile = {
    fullName: '',
    phoneNumber: '',
    email: '',
    role: ''
  };
  isEditable = false;

  constructor(
    private user: UserService,
    private dataService: DataService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.subscription = this.dataService.AccessToken.subscribe(
      (result) => (this.token = result)
    );
    this.getUser();
    this.profileForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      role: ['', Validators.required],
      // address: ['', Validators.required],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
    });
  }

  getUser() {
    this.user.getUserProfile().subscribe(
      (response) => {
        this.userProfile = response.data;
        this.profileForm.patchValue(this.userProfile);
      }
    );
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      console.log('Form Submitted:', this.profileForm.value);
    } else {
      console.log('Form is not valid');
    }
  }

  onEdit(): void {
    this.isEditable = true;
  }

  onCancel(): void {
    this.isEditable = false;
    this.profileForm.patchValue(this.userProfile);
  }
}
