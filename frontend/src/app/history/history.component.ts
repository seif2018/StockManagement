import { Component, OnInit } from '@angular/core';
import { ArticleService } from '../services/article.service';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltip } from '@angular/material/tooltip';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [
   CommonModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,MatTooltip,
    FormsModule
  ],
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {
  allMouvements: any[] = [];
  filteredMouvements: any[] = [];
  displayedColumns: string[] = ['date', 'referenceArticle', 'type', 'quantite', 'commentaire'];
  totalCount = 0;
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  sortBy = 'Date';
  descending = true;
  filterText = '';

  constructor(private articleService: ArticleService) { }

  ngOnInit(): void {
    this.loadMouvements();
  }

    loadMouvements(): void {
    this.articleService.getMouvementsPaged(
      this.currentPage,
      this.pageSize,
      this.sortBy,
      this.descending,
      this.filterText   // <-- envoi du filtre
    ).subscribe({
      next: (result) => {
        this.allMouvements = result.items;
        this.totalCount = result.totalCount;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
      },
      error: (err) => console.error(err)
    });
  }

    onSort(column: string): void {
    if (this.sortBy === column) {
      this.descending = !this.descending;
    } else {
      this.sortBy = column;
      this.descending = true;
    }
    this.loadMouvements();
  }

    previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadMouvements();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadMouvements();
    }
  }

  applyFilter(): void {
    this.currentPage = 1; // on revient à la première page lors d'un nouveau filtre
    this.loadMouvements();
  }

  exportInventaires(): void {
  this.articleService.exportInventairesCSV().subscribe({
    next: (data) => {
      const headers = ['Référence', 'Nom article', 'Prix HT', 'Prix TTC', 'Type', 'Ancienne qté', 'Nouvelle qté', 'Écart', 'Date'];
      const rows = data.map(item => [
        item.referenceArticle,
        item.nomArticle,
        item.prixHT,
        item.prixTTC,
        item.typeArticle,
        item.ancienneQuantite,
        item.nouvelleQuantite,
        item.ecart,
        new Date(item.date).toLocaleString()
      ]);
      const csvContent = [headers.join(';'), ...rows.map(row => row.join(';'))].join('\n');
      const blob = new Blob(['\uFEFF' + csvContent], { type: 'text/csv;charset=utf-8;' });
      const url = URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `inventaires_${new Date().toISOString().slice(0, 19)}.csv`;
      link.click();
      URL.revokeObjectURL(url);
    },
    error: (err) => console.error(err)
  });
}

  // Méthode pour rafraîchir l'historique depuis l'extérieur (appelée par AppComponent après chaque action)
  refresh(): void {
    this.loadMouvements();
  }
}