import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ApiService } from './core/services/api.service';
import { Vet } from './core/models/vet.model';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  template: `
    <h1>VetiCare Frontend</h1>
    <p>Conexión a la API: {{ connectionStatus }}</p>
    @if (vets.length > 0) {
      <ul>
        @for (vet of vets; track vet.id) {
          <li>Dr. {{ vet.firstName }} {{ vet.lastName }} — {{ vet.specialty }}</li>
        }
      </ul>
    }
    <router-outlet />
  `,
  styleUrl: './app.scss'
})
export class App implements OnInit {
  private apiService = inject(ApiService);
  private cdr = inject(ChangeDetectorRef);

  connectionStatus = 'Verificando...';
  vets: Vet[] = [];

  ngOnInit(): void {
    this.apiService.get<Vet[]>('vets').subscribe({
      next: (vets) => {
        this.connectionStatus = '✔ Conectado exitosamente';
        this.vets = vets;
        this.cdr.detectChanges();
        console.log('Vets:', this.vets);
      },
      error: (err) => {
        this.connectionStatus = '✘ Error de conexión — revisa que el backend esté corriendo';
        this.cdr.detectChanges();
        console.error('Error:', err);
      }
    });
  }
}
