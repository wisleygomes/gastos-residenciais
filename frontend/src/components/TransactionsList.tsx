import { Transaction, TransactionType } from '../types';

interface TransactionsListProps {
  transactions: Transaction[];
}

const currencyFormatter = new Intl.NumberFormat('pt-BR', {
  style: 'currency',
  currency: 'BRL',
});

/** Lista somente-leitura das transações cadastradas (mais recentes primeiro). */
export default function TransactionsList({ transactions }: TransactionsListProps) {
  if (transactions.length === 0) {
    return <p className="empty-state">Nenhuma transação cadastrada ainda.</p>;
  }

  return (
    <table className="data-table">
      <thead>
        <tr>
          <th>Data</th>
          <th>Pessoa</th>
          <th>Descrição</th>
          <th>Tipo</th>
          <th>Valor</th>
        </tr>
      </thead>
      <tbody>
        {transactions.map((t) => (
          <tr key={t.id}>
            <td>{new Date(t.createdAt).toLocaleString('pt-BR')}</td>
            <td>{t.personName}</td>
            <td>{t.description}</td>
            <td>
              {t.type === TransactionType.Income ? (
                <span className="badge badge-success">Receita</span>
              ) : (
                <span className="badge badge-expense">Despesa</span>
              )}
            </td>
            <td className={t.type === TransactionType.Income ? 'value-income' : 'value-expense'}>
              {currencyFormatter.format(t.value)}
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
