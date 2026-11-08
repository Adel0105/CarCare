import { DatePipe, DecimalPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, OnInit, signal } from '@angular/core';

import { ExpenseCategory } from '../expenses/expense';
import { Dashboard, DashboardExpenseCategory, DashboardMonthlyExpense } from './dashboard';
import { DashboardService } from './dashboard.service';

@Component({
  selector: 'app-dashboard',
  imports: [DatePipe, DecimalPipe],
  styleUrl: './dashboard.component.scss',
  template: `
    <section class="dashboard-page">
      <header class="dashboard-header">
        <p class="dashboard-eyebrow">CarCare overview</p>
        <h1>Dashboard</h1>
        <p class="dashboard-subtitle">
          Pregled vozila, troškova i održavanja na jednom mjestu.
        </p>
      </header>

      @if (isLoading()) {
        <p class="empty-message">Učitavanje dashboarda...</p>
      } @else if (error(); as errorMessage) {
        <p class="empty-message error">{{ errorMessage }}</p>
      } @else if (dashboard(); as data) {
        <div class="stats-grid">
          <article class="stat-card accent-vehicles">
            <div class="stat-icon" aria-hidden="true">V</div>
            <div class="stat-body">
              <h2 class="stat-label">Ukupno vozila</h2>
              <p class="stat-value">{{ data.totalVehicles }}</p>
            </div>
          </article>

          <article class="stat-card accent-total">
            <div class="stat-icon" aria-hidden="true">Σ</div>
            <div class="stat-body">
              <h2 class="stat-label">Ukupni troškovi</h2>
              <p class="stat-value">{{ data.totalExpensesAmount | number: '1.2-2' }} KM</p>
            </div>
          </article>

          <article class="stat-card accent-month">
            <div class="stat-icon" aria-hidden="true">M</div>
            <div class="stat-body">
              <h2 class="stat-label">Troškovi ovog mjeseca</h2>
              <p class="stat-value">{{ data.currentMonthExpensesAmount | number: '1.2-2' }} KM</p>
            </div>
          </article>

          <article class="stat-card accent-reminders">
            <div class="stat-icon" aria-hidden="true">!</div>
            <div class="stat-body">
              <h2 class="stat-label">Aktivni podsjetnici</h2>
              <p class="stat-value">{{ data.activeRemindersCount }}</p>
            </div>
          </article>
        </div>

        <section
          class="overdue-section"
          [class.overdue-alert]="data.overdueRemindersCount > 0"
          [class.overdue-ok]="data.overdueRemindersCount === 0"
        >
          <div class="overdue-copy">
            <h2>Zakašnjeli podsjetnici</h2>
            <p class="overdue-note">
              @if (data.overdueRemindersCount > 0) {
                Potrebna pažnja — postoje podsjetnici s isteklim rokom.
              } @else {
                Sve je pod kontrolom — nema zakašnjelih podsjetnika.
              }
            </p>
          </div>
          <p class="overdue-count">{{ data.overdueRemindersCount }}</p>
        </section>

        <div class="charts-row">
          <section class="panel expenses-by-category">
            <div class="panel-header">
              <h2>Expenses by Category</h2>
              <p class="panel-subtitle">Udio troškova po kategorijama</p>
            </div>

            @if (data.expensesByCategory.length === 0) {
              <p class="empty-message">Nema evidentiranih troškova po kategorijama.</p>
            } @else {
              <ul class="category-list">
                @for (item of data.expensesByCategory; track item.category) {
                  <li class="category-row">
                    <div class="category-meta">
                      <span class="category-name">{{ categoryLabel(item.category) }}</span>
                      <span class="category-amount">{{ item.amount | number: '1.2-2' }} KM</span>
                    </div>
                    <div class="category-bar-track">
                      <div
                        class="category-bar-fill"
                        [style.width.%]="categoryBarPercent(item.amount, data.expensesByCategory)"
                      ></div>
                    </div>
                  </li>
                }
              </ul>
            }
          </section>

          <section class="panel monthly-expenses">
            <div class="panel-header">
              <h2>Monthly Expenses</h2>
              <p class="panel-subtitle">Posljednjih šest mjeseci</p>
            </div>

            @if (!hasMonthlyExpenses(data.monthlyExpenses)) {
              <p class="empty-message">Nema evidentiranih troškova u posljednjih šest mjeseci.</p>
            } @else {
              <div class="monthly-chart">
                @for (item of data.monthlyExpenses; track item.year + '-' + item.month) {
                  <div class="monthly-column">
                    <span class="monthly-amount">{{ item.amount | number: '1.0-0' }} KM</span>
                    <div class="monthly-bar-track">
                      <div
                        class="monthly-bar-fill"
                        [class.is-zero]="item.amount === 0"
                        [style.height.%]="monthlyBarPercent(item.amount, data.monthlyExpenses)"
                      ></div>
                    </div>
                    <span class="monthly-label">{{ monthLabel(item.month) }}</span>
                  </div>
                }
              </div>
            }
          </section>
        </div>

        <section class="recent-maintenance">
          <div class="panel-header">
            <h2>Nedavno održavanje</h2>
            <p class="panel-subtitle">Posljednje aktivnosti servisa i popravki</p>
          </div>

          @if (data.recentMaintenance.length === 0) {
            <p class="empty-message">Nema evidentiranih aktivnosti održavanja.</p>
          } @else {
            <div class="activity-list">
              @for (activity of data.recentMaintenance; track activity.id) {
                <article class="activity-card">
                  <div class="activity-top">
                    <h3 class="activity-title">{{ activity.title }}</h3>
                    <p class="activity-cost">{{ activity.cost | number: '1.2-2' }} KM</p>
                  </div>
                  <dl class="activity-details">
                    <div>
                      <dt>Vozilo</dt>
                      <dd>{{ activity.vehicleLabel }}</dd>
                    </div>
                    <div>
                      <dt>Datum</dt>
                      <dd>{{ activity.date | date: 'dd.MM.yyyy' }}</dd>
                    </div>
                    <div>
                      <dt>Tip</dt>
                      <dd>
                        @switch (activity.type) {
                          @case (0) {
                            Servis
                          }
                          @case (1) {
                            Popravka
                          }
                          @case (2) {
                            Ostalo
                          }
                          @default {
                            {{ activity.type }}
                          }
                        }
                      </dd>
                    </div>
                  </dl>
                </article>
              }
            </div>
          }
        </section>
      }
    </section>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent implements OnInit {
  private readonly dashboardService = inject(DashboardService);

  protected readonly dashboard = signal<Dashboard | null>(null);
  protected readonly isLoading = signal(true);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadDashboard();
  }

  protected categoryLabel(category: ExpenseCategory): string {
    switch (category) {
      case ExpenseCategory.Fuel:
        return 'Fuel';
      case ExpenseCategory.Insurance:
        return 'Insurance';
      case ExpenseCategory.Registration:
        return 'Registration';
      case ExpenseCategory.Parking:
        return 'Parking';
      case ExpenseCategory.Other:
        return 'Other';
    }
  }

  protected categoryBarPercent(
    amount: number,
    categories: DashboardExpenseCategory[],
  ): number {
    const maxAmount = Math.max(...categories.map((item) => item.amount), 0);
    if (maxAmount <= 0) {
      return 0;
    }

    return (amount / maxAmount) * 100;
  }

  protected hasMonthlyExpenses(months: DashboardMonthlyExpense[]): boolean {
    return months.some((item) => item.amount > 0);
  }

  protected monthLabel(month: number): string {
    const labels = ['Jan', 'Feb', 'Mar', 'Apr', 'Maj', 'Jun', 'Jul', 'Avg', 'Sep', 'Okt', 'Nov', 'Dec'];
    return labels[month - 1] ?? '';
  }

  protected monthlyBarPercent(amount: number, months: DashboardMonthlyExpense[]): number {
    const maxAmount = Math.max(...months.map((item) => item.amount), 0);
    if (maxAmount <= 0 || amount <= 0) {
      return 0;
    }

    return (amount / maxAmount) * 100;
  }

  private loadDashboard(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.dashboardService.getDashboard().subscribe({
      next: (data) => {
        this.dashboard.set(data);
        this.isLoading.set(false);
        this.error.set(null);
      },
      error: () => {
        this.isLoading.set(false);
        this.error.set('Greška pri učitavanju dashboarda. Pokušaj ponovo.');
      },
    });
  }
}
