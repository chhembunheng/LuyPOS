import Image from "next/image";
import {
  Bell,
  Boxes,
  ClipboardList,
  CreditCard,
  LayoutDashboard,
  PackageCheck,
  ReceiptText,
  Search,
  Settings,
  ShoppingCart,
  UsersRound,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";

const metrics = [
  { label: "Total Sales", value: "$12,450.00", delta: "+8.2%", icon: ReceiptText },
  { label: "Orders", value: "320", delta: "+4.8%", icon: ShoppingCart },
  { label: "Products", value: "2,145", delta: "+10.1%", icon: Boxes },
  { label: "Customers", value: "1,280", delta: "+5.7%", icon: UsersRound },
];

const orders = [
  { id: "#ORD-1001", customer: "Sophea Market", amount: "$225.50", status: "Paid" },
  { id: "#ORD-1002", customer: "Dara Coffee", amount: "$89.20", status: "Paid" },
  { id: "#ORD-1003", customer: "Malis Shop", amount: "$310.40", status: "Pending" },
  { id: "#ORD-1004", customer: "Nika Mart", amount: "$156.00", status: "Paid" },
];

const navItems = [
  { label: "Dashboard", icon: LayoutDashboard },
  { label: "Orders", icon: ClipboardList },
  { label: "Products", icon: PackageCheck },
  { label: "Payments", icon: CreditCard },
  { label: "Settings", icon: Settings },
];

export default function DashboardPage() {
  return (
    <main className="min-h-dvh bg-[#F6F5EE] text-[#182748]">
      <div className="grid min-h-dvh lg:grid-cols-[17rem_1fr]">
        <aside className="hidden border-r border-[#B8C1CF]/60 bg-[#182748] px-5 py-6 text-white lg:block">
          <Image
            src="/luypos-brand.png"
            alt="LuyPOS"
            width={180}
            height={180}
            className="h-28 w-28 object-contain"
            priority
          />
          <nav className="mt-10 space-y-1">
            {navItems.map((item, index) => (
              <Button
                key={item.label}
                variant={index === 0 ? "default" : "ghost"}
                className={`w-full justify-start ${index === 0 ? "" : "text-[#B8C1CF] hover:bg-white/10 hover:text-white"}`}
              >
                <item.icon aria-hidden />
                {item.label}
              </Button>
            ))}
          </nav>
        </aside>

        <section className="px-5 py-5 sm:px-8">
          <header className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
            <div>
              <p className="text-sm font-semibold uppercase tracking-[0.14em] text-[#FF843D]">
                Dashboard
              </p>
              <h1 className="mt-1 text-3xl font-semibold tracking-normal">Hello, Admin</h1>
            </div>
            <div className="flex items-center gap-3">
              <div className="relative hidden sm:block">
                <Search className="pointer-events-none absolute left-3 top-1/2 size-4 -translate-y-1/2 text-[#7C8495]" />
                <Input className="w-64 bg-white pl-9" placeholder="Search orders" />
              </div>
              <Button variant="outline" size="icon" aria-label="Notifications">
                <Bell aria-hidden />
              </Button>
            </div>
          </header>

          <div className="mt-6 grid gap-4 md:grid-cols-2 xl:grid-cols-4">
            {metrics.map((metric) => (
              <Card key={metric.label} className="p-5">
                <div className="flex items-start justify-between gap-4">
                  <div>
                    <p className="text-sm text-[#7C8495]">{metric.label}</p>
                    <div className="mt-2 text-2xl font-semibold">{metric.value}</div>
                    <div className="mt-2 text-sm font-medium text-[#FF843D]">{metric.delta}</div>
                  </div>
                  <div className="rounded-md bg-[#FBCBA7]/40 p-2 text-[#FF843D]">
                    <metric.icon aria-hidden />
                  </div>
                </div>
              </Card>
            ))}
          </div>

          <div className="mt-6 grid gap-6 xl:grid-cols-[1.35fr_0.65fr]">
            <Card>
              <CardHeader className="p-5">
                <CardTitle>Sales Overview</CardTitle>
                <CardDescription>Dummy revenue trend for this week.</CardDescription>
              </CardHeader>
              <CardContent className="px-5 pb-5">
                <div className="grid h-64 grid-cols-7 items-end gap-3 border-b border-[#B8C1CF]/50">
                  {[72, 108, 86, 145, 124, 172, 156].map((height, index) => (
                    <div key={index} className="rounded-t-md bg-[#FF843D]" style={{ height }} />
                  ))}
                </div>
              </CardContent>
            </Card>

            <Card>
              <CardHeader className="p-5">
                <CardTitle>Sales Mix</CardTitle>
                <CardDescription>Top categories.</CardDescription>
              </CardHeader>
              <CardContent className="space-y-4 px-5 pb-5">
                {[
                  ["Electronics", "45%"],
                  ["Accessories", "25%"],
                  ["Fashion", "20%"],
                  ["Other", "10%"],
                ].map(([label, value]) => (
                  <div key={label}>
                    <div className="mb-2 flex justify-between text-sm">
                      <span>{label}</span>
                      <span className="font-semibold">{value}</span>
                    </div>
                    <div className="h-2 rounded-full bg-[#B8C1CF]/35">
                      <div className="h-full rounded-full bg-[#FF843D]" style={{ width: value }} />
                    </div>
                  </div>
                ))}
              </CardContent>
            </Card>
          </div>

          <Card className="mt-6">
            <CardHeader className="p-5">
              <CardTitle>Recent Orders</CardTitle>
              <CardDescription>Dummy order activity.</CardDescription>
            </CardHeader>
            <CardContent className="px-5 pb-5">
              <div className="overflow-hidden rounded-md border border-[#B8C1CF]/60">
                {orders.map((order) => (
                  <div
                    key={order.id}
                    className="grid grid-cols-4 gap-3 border-b border-[#B8C1CF]/40 bg-white px-4 py-3 text-sm last:border-b-0"
                  >
                    <span className="font-semibold">{order.id}</span>
                    <span>{order.customer}</span>
                    <span>{order.amount}</span>
                    <span className="font-medium text-[#FF843D]">{order.status}</span>
                  </div>
                ))}
              </div>
            </CardContent>
          </Card>
        </section>
      </div>
    </main>
  );
}
