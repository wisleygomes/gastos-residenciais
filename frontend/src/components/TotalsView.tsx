import type { TotalsReport } from '../types';

interface TotalsViewProps {
  report: TotalsReport | null;
}

const currencyFormatter = new Intl.NumberFormat('pt-BR', {
  style: 'currency',
  currency: 'BRL',
});

/**
 * Exibe os totais de receita, despesa e saldo por pessoa, seguidos do
 * total geral consolidado de todas as pessoas cadastradas.
 */
export default function TotalsView({ report }: TotalsViewProps) {
  if (!report) {
    return <p className="empty-state">Carregando totais...</p>;
  }

  if (report.people.length === 0) {
    return <p className="empty-state">Nenhuma pessoa cadastrada ainda.</p>;
  }

  return (
    <table className="data-table">
      <thead>
        <tr>
          <th>Pessoa</th>
          <th>Total de receitas</th>
          <th>Total de despesas</th>
          <th>Saldo</th>
        </tr>
      </thead>
      <tbody>
        {report.people.map((p) => (
          <tr key={p.personId}>
            <td>{p.personName}</td>
            <td className="value-income">{currencyFormatter.format(p.totalIncome)}</td>
            <td className="value-expense">{currencyFormatter.format(p.totalExpense)}</td>
            <td className={p.balance >= 0 ? 'value-income' : 'value-expense'}>
              {currencyFormatter.format(p.balance)}
            </td>
          </tr>
        ))}
      </tbody>
      <tfoot>
        <tr className="totals-row">
          <td>Total geral</td>
          <td className="value-income">{currencyFormatter.format(report.grandTotalIncome)}</td>
          <td className="value-expense">{currencyFormatter.format(report.grandTotalExpense)}</td>
          <td className={report.grandTotalBalance >= 0 ? 'value-income' : 'value-expense'}>
            {currencyFormatter.format(report.grandTotalBalance)}
          </td>
        </tr>
      </tfoot>
    </table>
  );
}
