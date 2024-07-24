import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-health-check',
  templateUrl: './health-check.component.html',
  styleUrl: './health-check.component.scss'
})
export class HealthCheckComponent {
  public result?: Result;
  constructor(private http: HttpClient) {

  }

  ngOnInit() {
    this.http.get<Result>(environment.baseUrl + 'api/health').subscribe({
      next: (result) => this.result = result,
      error: (error) => console.error(error),
     complete: () => console.info('complete')
    });
  }

}
interface Result {
  checks: Check[];
  totalStatus: string;
  totalResponseTime: number;
}
interface Check {
  name: string;
  responseTime: number;
  status: string;
  description: string;

}
