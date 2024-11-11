import { NgFor } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  
  http = inject(HttpClient);
  title = 'MyApp';
  users:any;
  ngOnInit():void {
    this.http.get('http://localhost:5082/api/User/AllUsers').subscribe(
      {
        next: response => this.users=response,
        error: error => console.log(error),
        complete: () => console.log('request completed')
      }
    )
  }
}
