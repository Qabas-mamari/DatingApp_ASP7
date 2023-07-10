import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //child will receive component from parent
  @Output() cancelRegister = new EventEmitter();

  model:any = {}

  constructor(private accountService: AccountService, private toastr: ToastrService) {}
  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.model).subscribe({
      // next: response =>{
      //   console.log(response);
      //   this.cancel();
      // },
      next: () =>{
        this.cancel();
      },
      error: error => this.toastr.error(error.error)
    }
    )
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}
